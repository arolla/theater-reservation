namespace TheaterReservation.Domain.Reservation;

public class TheaterSession
{
    public TheaterSession(string title, DateTime startTime)
    {
        Title = title;
        StartTime = startTime;
    }

    public DateTime StartTime { get; }

    public string Title { get; }
}