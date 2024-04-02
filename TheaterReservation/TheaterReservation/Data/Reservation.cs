namespace TheaterReservation.Data;

public class Reservation
{
    private Int64 reservationId;

    private Int64 performanceId;

    private String status;

    private String[] seats;

    public Int64 GetReservationId()
    {
        return reservationId;
    }

    public void SetReservationId(Int64 reservationId)
    {
        this.reservationId = reservationId;
    }

    public Int64 GetPerformanceId()
    {
        return performanceId;
    }

    public void SetPerformanceId(Int64 performanceId)
    {
        this.performanceId = performanceId;
    }

    public String GetStatus()
    {
        return status;
    }

    public void SetStatus(String status)
    {
        this.status = status;
    }

    public String[] GetSeats()
    {
        return seats;
    }

    public void SetSeats(String[] seats)
    {
        this.seats = seats;
    }
    
    public override String ToString()
    {
        return "Reservation{" +
               "reservationId=" + reservationId +
               ", status='" + status + '\'' +
               ", seats=" + seats +
               '}';
    }
}