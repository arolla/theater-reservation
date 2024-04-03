using System.Globalization;
using System.Text;
using TheaterReservation.Data;

namespace TheaterReservation;

public class ReservationRequest
{
    public ReservationRequest(string reservationCategory, Performance performance, string reservationId, List<string> foundSeats, Dictionary<string, string> seatsCategory, string totalBilling)
    {
        ReservationCategory = reservationCategory;
        Performance = performance;
        ReservationId = reservationId;
        FoundSeats = foundSeats;
        SeatsCategory = seatsCategory;
        TotalBilling = totalBilling;
    }

    public string ReservationCategory { get; }
    public Performance Performance { get; }
    public string ReservationId { get; }
    public List<string> FoundSeats { get; }
    public Dictionary<string, string> SeatsCategory { get; }
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
        return FoundSeats.Count != 0;
    }

    public string GetSeatCategory(string seatReference)
    {
        return SeatsCategory[seatReference];
    }
}