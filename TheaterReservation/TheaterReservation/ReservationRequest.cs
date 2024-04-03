using System.Text;
using TheaterReservation.Data;

namespace TheaterReservation;

public class ReservationRequest
{
    public ReservationRequest(string reservationCategory, Performance performance, StringBuilder sb, string res_id, List<string> foundSeats, Dictionary<string, string> seatsCategory, string total)
    {
        ReservationCategory = reservationCategory;
        Performance = performance;
        Sb = sb;
        ResId = res_id;
        FoundSeats = foundSeats;
        SeatsCategory = seatsCategory;
        Total = total;
    }

    public string ReservationCategory { get; }
    public Performance Performance { get; }
    public StringBuilder Sb { get; }
    public string ResId { get; }
    public List<string> FoundSeats { get; }
    public Dictionary<string, string> SeatsCategory { get; }
    public string Total { get; }
}