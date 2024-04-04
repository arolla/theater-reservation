namespace TheaterReservation.Domain.Topology;

public class SeatTopology
{
    public String SeatReference { get; }

    public String category { get; }

    public SeatTopology(string seatReference, string category)
    {
        this.SeatReference = seatReference;
        this.category = category;
    }
}