using TheaterReservation.Dao;
using TheaterReservation.Data;
using TheaterReservation.Domain;
using TheaterReservation.Domain.Allocation;
using TheaterReservation.Exposition;

namespace TheaterReservation;

public class TheaterService
{
    private readonly TheaterRoomDao theaterRoomDao = new TheaterRoomDao();
    private readonly PerformancePriceDao performancePriceDao = new PerformancePriceDao();
    private readonly AllocationQuotas allocationQuotas;

    public TheaterService(AllocationQuotas quotas)
    {
        allocationQuotas = quotas;
    }

    public ReservationRequest Reserve(long customerId, int reservationCount, string reservationCategory,
        Performance performance)
    {
        CustomerSubscriptionDao customerSubscriptionDao = new CustomerSubscriptionDao();
        bool isSubscribed = customerSubscriptionDao.FetchCustomerSubscription(customerId);
        var voucherProgramDiscount = VoucherProgramDao.FetchVoucherProgram(performance.startTime);
        var performancePrice = performancePriceDao.FetchPerformancePrice(performance.id);
        TheaterRoom room = theaterRoomDao.FetchTheaterRoom(performance.id);
        String res_id = ReservationService.InitNewReservation();

        Reservation reservation = new Reservation();
        reservation.SetReservationId(Convert.ToInt64(res_id));
        reservation.SetPerformanceId(performance.id);
        String zoneCategory;
        int remainingSeats = 0;
        int totalSeats = 0;
        bool foundAllSeats = false;
        List<ReservationSeat> reservedSeats = new List<ReservationSeat>();
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
                                    reservedSeats.Add(new ReservationSeat(seat, zoneCategory));
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
            }
        }

        var allocationQuotaSpecification = allocationQuotas.GetVipQuotaSpecification(performance);
        PerformanceInventory performanceInventory = new PerformanceInventory(remainingSeats, totalSeats);
        if (allocationQuotaSpecification.IsSatisfiedBy(performanceInventory))
        {
            reservedSeats = new List<ReservationSeat>();
        }

        reservation.SetSeats(reservedSeats.Select(r => r.Seat).ToArray());

        // calculate raw price
        Amount myPrice = new Amount(performancePrice);

        Amount intialPrice = Amount.Nothing();
        foreach (var reservedSeat in reservedSeats)
        {
            Rate categoryRatio = reservedSeat.Category.Equals("STANDARD") ? Rate.Fully() : new Rate("1.5");
            intialPrice = intialPrice.Add(myPrice.Apply(categoryRatio));
        }

        // check and apply discounts and fidelity program
        Rate discountTime = new Rate(voucherProgramDiscount);


        Amount totalBilling = new Amount(intialPrice);
        if (isSubscribed)
        {
            var subtract = Rate.DiscountPercent("17.5");
            totalBilling = totalBilling.Apply(subtract);
        }

        Rate discountRatio = Rate.Fully().Subtract(discountTime);
        String total = totalBilling.Apply(discountRatio).AsString() + "€";

        if (foundAllSeats)
        {
            theaterRoomDao.SaveSeats(performance.id,
                reservedSeats.Select(r => r.Seat).ToList()
                , "BOOKING_PENDING");
        }
        ReservationService.UpdateReservation(reservation);

        var reservationRequest = new ReservationRequest(reservationCategory, performance, res_id, reservedSeats, total);
        return reservationRequest;
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
        TheaterService theaterService = new TheaterService(new AllocationQuotas());
        TicketPrinter ticketPrinter = new TicketPrinter(theaterService);
        Console.WriteLine(ticketPrinter.Reservation(1L, 4, "STANDARD",
            performance));

        Console.WriteLine(ticketPrinter.Reservation(1L, 5, "STANDARD",
            performance));

        Performance performance2 = new Performance();
        performance2.id = 2L;
        performance2.play = "Les fourberies de Scala - Molière";
        performance2.startTime = new DateTime(2023, 03, 21, 21, 0, 0);
        performance2.performanceNature = "PREVIEW";
        Console.WriteLine(ticketPrinter.Reservation(2L, 4, "STANDARD",
            performance2));
    }
}