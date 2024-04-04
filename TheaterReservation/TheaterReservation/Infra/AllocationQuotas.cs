using TheaterReservation.Domain.Allocation;

namespace TheaterReservation.Infra;

public class AllocationQuotas : IAllocationQuotas
{
    public AllocationQuotaSpecification Find(PerformanceNature performanceNature)
    {
        var vipQuota = GetVipQuota(performanceNature);
        var allocationQuotaSpecification = new AllocationQuotaSpecification(vipQuota);
        return allocationQuotaSpecification;
    }

    private double GetVipQuota(PerformanceNature performanceNature)
    {
        var performancePerformanceNature = performanceNature.Value;
        switch (performancePerformanceNature)
        {
            case "PREMIERE":
                return 0.5;
            case "PREVIEW":
                return 0.9;
            default:
                return -1;
        }
    }
}