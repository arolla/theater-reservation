using System.Text;
using TheaterReservation.Dao;
using TheaterReservation.Data;
using TheaterReservation.Domain;

namespace TheaterReservation
{

    public class TheaterService
    {
        private readonly TheaterRoomDao theaterRoomDao = new TheaterRoomDao();
        private readonly PerformancePriceDao performancePriceDao = new PerformancePriceDao();

        bool debug = false;


        public String Reservation(Int64 customerId, int reservationCount, String reservationCategory, Performance performance)
        {
            Reservation reservation = new Reservation();
            StringBuilder sb = new StringBuilder();
            int bookedSeats = 0;
            List<String> foundSeats = new List<string>();
            Dictionary<String, String> seatsCategory = new Dictionary<string, string>();
            String zoneCategory;
            int remainingSeats = 0;
            int totalSeats = 0;
            bool foundAllSeats = false;

            sb.Append("<reservation>\n");
            sb.Append("\t<performance>\n");
            sb.Append("\t\t<play>").Append(performance.play).Append("</play>\n");
            sb.Append("\t\t<date>").Append(performance.startTime.ToString()).Append("</date>\n");
            sb.Append("\t</performance>\n");

            String res_id = ReservationService.InitNewReservation();
            reservation.SetReservationId(Convert.ToInt64(res_id));
            reservation.SetPerformanceId(performance.id);
            sb.Append("\t<reservationId>").Append(res_id).Append("</reservationId>\n");

            TheaterRoom room = theaterRoomDao.FetchTheaterRoom(performance.id);

            // find "reservationCount" first contiguous seats in any row
            for (int i = 0; i < room.GetZones().Length; i++)
            {
                Zone zone = room.GetZones()[i];
                zoneCategory = zone.GetCategory();
                for (int j = 0; j < zone.GetRows().Length; j++)
                {
                    Row row = zone.GetRows()[j];
                    List<String> seatsForRow = new List<string>();
                    int streakOfNotReservedSeats = 0;
                    for (int k = 0; k < row.GetSeats().Length; k++)
                    {
                        totalSeats++; // devrait être dans une série de boucles différentes mais ça permet qq ns
                        Seat aSeat = row.GetSeats()[k];
                        if (!aSeat.GetStatus().Equals("BOOKED") && !aSeat.GetStatus().Equals("BOOKING_PENDING"))
                        {
                            remainingSeats++;
                            if (!reservationCategory.Equals(zoneCategory))
                            {
                                continue;
                            }
                            if (!foundAllSeats)
                            {
                                seatsForRow.Add(aSeat.GetSeatId());
                                streakOfNotReservedSeats++;
                                if (streakOfNotReservedSeats >= reservationCount)
                                {
                                    foreach (String seat in seatsForRow)
                                    {
                                        foundSeats.Add(seat);
                                        seatsCategory.Add(seat, zoneCategory);
                                    }
                                    foundAllSeats = true;
                                    remainingSeats -= streakOfNotReservedSeats;
                                }
                            }
                        }
                        else
                        {
                            seatsForRow = new List<string>();
                            streakOfNotReservedSeats = 0;
                        }
                    }
                    if (foundAllSeats)
                    {
                        for (int k = 0; k < row.GetSeats().Length; k++)
                        {
                            Seat seat = row.GetSeats()[k];
                            bookedSeats++;
                            if (foundSeats.Contains(seat.GetSeatId()))
                            {
                                if (debug)
                                {
                                    Console.WriteLine("MIAOU!!! : Seat " + seat.GetSeatId() + " will be saved as PENDING");
                                }
                            }
                        }

                        theaterRoomDao.SaveSeats(performance.id, foundSeats, "BOOKING_PENDING");
                    }
                }
            }
            reservation.SetSeats(foundSeats.ToArray());

            Console.WriteLine(remainingSeats);
            Console.WriteLine(totalSeats);
            if (foundAllSeats)
            {
                reservation.SetStatus("PENDING");
            }
            else
            {
                reservation.SetStatus("ABORTED");
            }

            ReservationService.UpdateReservation(reservation);

            if (performance.performanceNature.Equals("PREMIERE") && remainingSeats < totalSeats * 0.5)
            {
                // keep 50% seats for VIP
                foundSeats = new List<string>();
                Console.WriteLine("Not enough VIP seats available for Premiere");
            }
            else if (performance.performanceNature.Equals("PREVIEW") && remainingSeats < totalSeats * 0.9)
            {
                // keep 10% seats for VIP
                foundSeats = new List<string>();
                Console.WriteLine("Not enough VIP seats available for Preview");
            }


            if (foundSeats.Count != 0)
            {
                sb.Append("\t<reservationStatus>FULFILLABLE</reservationStatus>\n");
                sb.Append("\t\t<seats>\n");
                foreach (String s in foundSeats)
                {
                    sb.Append("\t\t\t<seat>\n");
                    sb.Append("\t\t\t\t<id>").Append(s).Append("</id>\n");
                    sb.Append("\t\t\t\t<category>").Append(seatsCategory[s]).Append("</category>\n");
                    sb.Append("\t\t\t</seat>\n");
                }
                sb.Append("\t\t</seats>\n");
            }
            else
            {
                sb.Append("\t<reservationStatus>ABORTED</reservationStatus>\n");
            }
            
            const decimal zero = 0m;
            const decimal one = 1m;
            Amount adjustedPrice = Amount.Nothing();

            // calculate raw price
            Amount myPrice = new Amount(performancePriceDao.FetchPerformancePrice(performance.id));

            Amount intialprice = Amount.Nothing();
            foreach (String foundSeat in foundSeats)
            {
                Rate categoryRatio = seatsCategory[foundSeat].Equals("STANDARD") ? Rate.Fully() : new Rate("1.5");
                intialprice = intialprice.Add(myPrice.Apply(categoryRatio));
            }

            // check and apply discounts and fidelity program
            Rate discountTime = new Rate(VoucherProgramDao.FetchVoucherProgram(performance.startTime));

            // has he subscribed or not
            CustomerSubscriptionDao customerSubscriptionDao = new CustomerSubscriptionDao();
            bool isSubscribed = customerSubscriptionDao.FetchCustomerSubscription(customerId);

            Amount totalBilling = new Amount(intialprice);
            if (isSubscribed)
            {
                // apply a 25% discount when the user is subscribed
                var subtract =  Rate.DiscountPercent("17.5");
                totalBilling = totalBilling.Apply(subtract);
            }
            Rate discountRatio = Rate.Fully().Subtract(discountTime);
            String total = totalBilling.Apply(discountRatio).AsString() + "€";

            sb.Append("\t<seatCategory>").Append(reservationCategory).Append("</seatCategory>\n");
            sb.Append("\t<totalAmountDue>").Append(total).Append("</totalAmountDue>\n");
            sb.Append("</reservation>\n");
            return sb.ToString();
        }

        public void CancelReservation(String reservationId, Int64 performanceId, List<String> seats)
        {
            TheaterRoom theaterRoom = theaterRoomDao.FetchTheaterRoom(performanceId);
            for (int i = 0; i < theaterRoom.GetZones().Length; i++)
            {
                Zone zone = theaterRoom.GetZones()[i];
                for (int j = 0; j < zone.GetRows().Length; j++)
                {
                    Row row = zone.GetRows()[j];
                    for (int k = 0; k < row.GetSeats().Length; k++)
                    {
                        Seat seat = row.GetSeats()[k];
                        if (seats.Contains(seat.GetSeatId()))
                        {
                            seat.SetStatus("FREE");
                        }
                    }
                }
            }
            theaterRoomDao.Save(performanceId, theaterRoom);
            ReservationService.CancelReservation(Convert.ToInt64(reservationId));
        }


        public static void Main(String[] args)
        {
            Performance performance = new Performance();
            performance.id = 1L;
            performance.play = "The CICD by Corneille";
            performance.startTime = new DateTime(2023, 04, 22, 21, 0, 0);
            performance.performanceNature = "PREMIERE";
            TheaterService theaterService = new TheaterService();
            Console.WriteLine(theaterService.Reservation(1L, 4, "STANDARD",
                    performance));

            Console.WriteLine(theaterService.Reservation(1L, 5, "STANDARD",
                    performance));

            Performance performance2 = new Performance();
            performance2.id = 2L;
            performance2.play = "Les fourberies de Scala - Molière";
            performance2.startTime = new DateTime(2023, 03, 21, 21, 0, 0);
            performance2.performanceNature = "PREVIEW";
            Console.WriteLine(theaterService.Reservation(2L, 4, "STANDARD",
                    performance2));
        }
    }
}