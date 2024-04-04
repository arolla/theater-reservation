using TheaterReservation.Dao;
using TheaterReservation.Data;
using TheaterReservation.Domain;
using TheaterReservation.Domain.Allocation;
using TheaterReservation.Domain.Topology;
using TheaterReservation.Exposition;
using TheaterReservation.Infra;

namespace TheaterReservation.Domain.Reservation;

public class ReservationAgent
{
    private readonly TheaterRoomDao theaterRoomDao = new TheaterRoomDao();
    private readonly PerformancePriceDao performancePriceDao = new PerformancePriceDao();
    private readonly IAllocationQuotas allocationQuotas;

    public ReservationAgent(IAllocationQuotas quotas)
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
        string reservationId = ReservationService.InitNewReservation();
        var performanceNature = new PerformanceNature(performance.performanceNature);
        var allocationQuota = allocationQuotas.Find(performanceNature);

        Reservation reservation = new Reservation();
        reservation.SetReservationId(Convert.ToInt64(reservationId));
        reservation.SetPerformanceId(performance.id);

        TheaterTopology theaterTopology = TheaterTopology.From(room);
        PerformanceAllocation performanceAllocation = new PerformanceAllocation(theaterTopology, room.GetFreeSeats(), reservationCount, reservationCategory,
            allocationQuota);

        List<ReservationSeat> reservedSeats = performanceAllocation.FindSeatsForReservation();
        
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
        string total = totalBilling.Apply(discountRatio).AsString() + "€";

        if (reservedSeats.Count != 0)
        {
            theaterRoomDao.SaveSeats(performance.id,
                reservedSeats.Select(r => r.Seat).ToList()
                , "BOOKING_PENDING");
        }

        ReservationService.UpdateReservation(reservation);

        TheaterSession theaterSession = new TheaterSession(performance.play, performance.startTime);
        var reservationRequest = new ReservationRequest(reservationCategory, reservationId, reservedSeats, total, theaterSession);
        return reservationRequest;
    }

    public void CancelReservation(string reservationId, long performanceId, List<string> seats)
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


    public static void Main(string[] args)
    {
        Performance performance = new Performance();
        performance.id = 1L;
        performance.play = "The CICD by Corneille";
        performance.startTime = new DateTime(2023, 04, 22, 21, 0, 0);
        performance.performanceNature = "PREMIERE";
        ReservationAgent reservationAgent = new ReservationAgent(new AllocationQuotas());
        TicketPrinter ticketPrinter = new TicketPrinter(reservationAgent);
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