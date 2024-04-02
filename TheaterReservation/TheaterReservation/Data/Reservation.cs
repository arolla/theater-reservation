namespace TheaterReservation.Data;

public class Reservation
{

    private Int64 reservationId;

    private Int64 performanceId;

    private String status;

    private String[] seats;

    public Int64 getReservationId()
    {
        return reservationId;
    }

    public void setReservationId(Int64 reservationId)
    {
        this.reservationId = reservationId;
    }

    public Int64 getPerformanceId()
    {
        return performanceId;
    }

    public void setPerformanceId(Int64 performanceId)
    {
        this.performanceId = performanceId;
    }

    public String getStatus()
    {
        return status;
    }

    public void setStatus(String status)
    {
        this.status = status;
    }

    public String[] getSeats()
    {
        return seats;
    }

    public void setSeats(String[] seats)
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