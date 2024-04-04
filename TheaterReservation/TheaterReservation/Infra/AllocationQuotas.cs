using TheaterReservation.Domain.Allocation;

namespace TheaterReservation.Infra;

public class AllocationQuotas : IAllocationQuotas
{
    public AllocationQuotaSpecification GetVipQuota(PerformanceNature performanceNature)
    {
        var vipQuota = GetVipQuotaValue(performanceNature);
        var allocationQuotaSpecification = new AllocationQuotaSpecification(vipQuota);
        return allocationQuotaSpecification;
    }

    private double GetVipQuotaValue(PerformanceNature performanceNature)
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