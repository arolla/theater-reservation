namespace TheaterReservation.Data;

public class Seat
{

    private String seatId;
    private String status; // FREE, OCCUPIED

    public Seat(String seatId, String status)
    {
        this.seatId = seatId;
        this.status = status;
    }

    public String GetSeatId()
    {
        return seatId;
    }

    public void SetSeatId(String seatId)
    {
        this.seatId = seatId;
    }

    public String GetStatus()
    {
        return status;
    }

    public void SetStatus(String status)
    {
        this.status = status;
    }
        
    public override String ToString()
    {
        return "Seat{" +
               "seatId='" + seatId + '\'' +
               ", status='" + status + '\'' +
               '}';
    }
}