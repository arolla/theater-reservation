namespace TheaterReservation.Domain.Allocation;

public class AllocationQuotaSpecification : Specification<PerformanceInventory>
{
    private double shelvingQuota;

    public AllocationQuotaSpecification(double shelvingQuota)
    {
        this.shelvingQuota = shelvingQuota;
    }

    public bool IsSatisfiedBy(PerformanceInventory performanceInventory)
    {
        return performanceInventory.AvailableSeatsCount < performanceInventory.TotalSeatsCount * shelvingQuota;
    }
}