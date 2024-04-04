namespace TheaterReservation;

public class AllocationQuotaPredicate
{
    private int availableSeatsCount;
    private int totalSeatsCount;
    private double shelvingQuota;

    public AllocationQuotaPredicate(int availableSeatsCount, int totalSeatsCount, double shelvingQuota)
    {
        this.availableSeatsCount = availableSeatsCount;
        this.totalSeatsCount = totalSeatsCount;
        this.shelvingQuota = shelvingQuota;
    }

    public bool CanReserve()
    {
        return availableSeatsCount < totalSeatsCount * shelvingQuota;
    }
}