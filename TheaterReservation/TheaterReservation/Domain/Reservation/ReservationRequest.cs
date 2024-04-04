using System.Globalization;
using System.Text;

namespace TheaterReservation.Domain.Reservation;

public class ReservationRequest
{
    public ReservationRequest(string reservationCategory, string reservationId, List<ReservationSeat> reservedSeats, string totalBilling, TheaterSession theaterSession)
    {
        ReservationCategory = reservationCategory;
        ReservationId = reservationId;
        ReservedSeats = reservedSeats;
        TotalBilling = totalBilling;
        TheaterSession = theaterSession;
    }

    public string ReservationCategory { get; }
    public TheaterSession TheaterSession { get; }
    public string ReservationId { get; }
    public List<ReservationSeat> ReservedSeats { get; }
    public string TotalBilling { get; }

    public string GetPerformanceTitle()
    {
        return TheaterSession.Title;
    }

    public string GetStartDate()
    {
        return TheaterSession.StartTime.ToString();
    }

    public bool IsFulfillable()
    {
        return ReservedSeats.Count != 0;
    }
}