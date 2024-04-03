namespace TheaterReservation;

public class ReservationSeat
{
    public ReservationSeat(string seat, string zoneCategory)
    {
        this.Seat = seat;
        this.Category = zoneCategory;
    }

    public string Category { get; }
    public string Seat { get; }
}