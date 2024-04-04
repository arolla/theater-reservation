namespace TheaterReservation.Domain.Allocation;

public class AllocationQuotaCriteria
{
    public string PerformanceNature { get; }

    public AllocationQuotaCriteria(string performanceNature)
    {
        PerformanceNature = performanceNature;
    }
}