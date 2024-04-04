namespace TheaterReservation.Domain.Topology;

public class RowTopology
{
    public RowTopology(List<SeatTopology> seats)
    {
        this.seats = seats;
    }

    public List<SeatTopology> seats { get; }

    public int GetSeatCount()
    {
        return seats.Count();
    }

    public List<SeatTopology> FindSeatsForReservation(int reservationCount, String reservationCategory, List<String> freeSeats)
    {
        List<SeatTopology> reservableSeats = new List<SeatTopology>();
        foreach (SeatTopology seat in seats)
        {
            if (IsReservable(reservationCategory, freeSeats, seat))
            {
                reservableSeats.Add(seat);
            }
            else
            {
                reservableSeats = new List<SeatTopology>();
            }
            if (reservableSeats.Count == reservationCount)
            {
                return reservableSeats;
            }
        }
        return new List<SeatTopology>();
    }

    private bool IsReservable(String reservationCategory, List<String> freeSeats, SeatTopology seat)
    {
        return freeSeats.Contains(seat.SeatReference) && reservationCategory.Equals(seat.category);
    }
}