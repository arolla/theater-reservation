namespace TheaterReservation.Data;

public class Row
{
    private Seat[] seats;

    public Row(Seat[] seats)
    {
        this.seats = seats;
    }

    public Seat[] GetSeats()
    {
        return seats;
    }
}