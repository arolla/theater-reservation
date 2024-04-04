namespace TheaterReservation;

public class AllocationQuotaPredicate
{
    private int remainingSeats;
    private int totalSeats;
    private double vipQuota;

    public AllocationQuotaPredicate(int remainingSeats, int totalSeats, double vipQuota)
    {
        this.remainingSeats = remainingSeats;
        this.totalSeats = totalSeats;
        this.vipQuota = vipQuota;
    }

    public bool CanReserve()
    {
        return remainingSeats < totalSeats * vipQuota;
    }
}