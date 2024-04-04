namespace TheaterReservation.Domain.Reservation;

public class Reservation
{
    private long reservationId;

    private long performanceId;

    private string status;

    private string[] seats;

    public long GetReservationId()
    {
        return reservationId;
    }

    public void SetReservationId(long reservationId)
    {
        this.reservationId = reservationId;
    }

    public long GetPerformanceId()
    {
        return performanceId;
    }

    public void SetPerformanceId(long performanceId)
    {
        this.performanceId = performanceId;
    }

    public void SetStatus(string status)
    {
        this.status = status;
    }

    public string[] GetSeats()
    {
        return seats;
    }

    public void SetSeats(string[] seats)
    {
        this.seats = seats;
    }

    public override string ToString()
    {
        return "Reservation{" +
               "reservationId=" + reservationId +
               ", status='" + status + '\'' +
               ", seats=" + seats +
               '}';
    }
}