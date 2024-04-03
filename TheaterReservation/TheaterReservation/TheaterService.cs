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
        
        public String Reservation(Int64 customerId, int reservationCount, String reservationCategory, Performance performance)
        {
            Reservation reservation = new Reservation();
            List<String> foundSeats = new List<string>();
            Dictionary<String, String> seatsCategory = new Dictionary<string, string>();
            String zoneCategory;
            int remainingSeats = 0;
            int totalSeats = 0;
            bool foundAllSeats = false;
            
            String res_id = ReservationService.InitNewReservation();
            reservation.SetReservationId(Convert.ToInt64(res_id));
            reservation.SetPerformanceId(performance.id);
           
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
                        theaterRoomDao.SaveSeats(performance.id, foundSeats, "BOOKING_PENDING");
                    }
                }
            }
            reservation.SetSeats(foundSeats.ToArray());
            
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
                foundSeats = new List<string>();
            }
            else if (performance.performanceNature.Equals("PREVIEW") && remainingSeats < totalSeats * 0.9)
            {
                foundSeats = new List<string>();
            }
            
            // calculate raw price
            Amount myPrice = new Amount(performancePriceDao.FetchPerformancePrice(performance.id));

            Amount intialPrice = Amount.Nothing();
            foreach (var foundSeat in foundSeats)
            {
                Rate categoryRatio = seatsCategory[foundSeat].Equals("STANDARD") ? Rate.Fully() : new Rate("1.5");
                intialPrice = intialPrice.Add(myPrice.Apply(categoryRatio));
            }

            // check and apply discounts and fidelity program
            Rate discountTime = new Rate(VoucherProgramDao.FetchVoucherProgram(performance.startTime));
            
            CustomerSubscriptionDao customerSubscriptionDao = new CustomerSubscriptionDao();
            bool isSubscribed = customerSubscriptionDao.FetchCustomerSubscription(customerId);

            Amount totalBilling = new Amount(intialPrice);
            if (isSubscribed)
            {
                var subtract =  Rate.DiscountPercent("17.5");
                totalBilling = totalBilling.Apply(subtract);
            }
            Rate discountRatio = Rate.Fully().Subtract(discountTime);
            String total = totalBilling.Apply(discountRatio).AsString() + "€";

            return ToXml(new ReservationRequest(reservationCategory, performance, res_id, foundSeats, seatsCategory, total));
        }

        private static string ToXml(ReservationRequest reservationRequest)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<reservation>\n");
            sb.Append("\t<performance>\n");
            sb.Append("\t\t<play>").Append(reservationRequest.GetPerformanceTitle()).Append("</play>\n");
            sb.Append("\t\t<date>").Append(reservationRequest.GetStartDate()).Append("</date>\n");
            sb.Append("\t</performance>\n");
            sb.Append("\t<reservationId>").Append(reservationRequest.ReservationId).Append("</reservationId>\n");
            if (reservationRequest.IsFulfillable())
            {
                sb.Append("\t<reservationStatus>FULFILLABLE</reservationStatus>\n");
                sb.Append("\t\t<seats>\n");
                foreach (String seatReference in reservationRequest.FoundSeats)
                {
                    sb.Append("\t\t\t<seat>\n");
                    sb.Append("\t\t\t\t<id>").Append(seatReference).Append("</id>\n");
                    sb.Append("\t\t\t\t<category>").Append(reservationRequest.GetSeatCategory(seatReference)).Append("</category>\n");
                    sb.Append("\t\t\t</seat>\n");
                }

                sb.Append("\t\t</seats>\n");
            }
            else
            {
                sb.Append("\t<reservationStatus>ABORTED</reservationStatus>\n");
            }

            sb.Append("\t<seatCategory>").Append(reservationRequest.ReservationCategory).Append("</seatCategory>\n");
            sb.Append("\t<totalAmountDue>").Append(reservationRequest.TotalBilling).Append("</totalAmountDue>\n");
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