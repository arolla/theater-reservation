using System.Globalization;
using System.Text;
using TheaterReservation.Data;

namespace TheaterReservation;

public class ReservationRequest
{
    public ReservationRequest(string reservationCategory, Performance performance, string reservationId, List<ReservationSeat> reservedSeats, string totalBilling)
    {
        ReservationCategory = reservationCategory;
        Performance = performance;
        ReservationId = reservationId;
        ReservedSeats = reservedSeats;
        TotalBilling = totalBilling;
    }

    public string ReservationCategory { get; }
    public Performance Performance { get; }
    public string ReservationId { get; }
    public List<ReservationSeat> ReservedSeats { get; }
    public string TotalBilling { get; }

    public string GetPerformanceTitle()
    {
        return Performance.play;
    }

    public string GetStartDate()
    {
        return Performance.startTime.ToString();
    }

    public bool IsFulfillable()
    {
        return ReservedSeats.Count != 0;
    }
}