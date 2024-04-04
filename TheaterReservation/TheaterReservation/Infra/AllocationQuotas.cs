using TheaterReservation.Domain.Allocation;

namespace TheaterReservation.Infra;

public class AllocationQuotas : IAllocationQuotas
{
    public AllocationQuotaSpecification GetVipQuota(AllocationQuotaCriteria allocationQuotaCriteria)
    {
        var vipQuota = GetVipQuotaValue(allocationQuotaCriteria);
        var allocationQuotaSpecification = new AllocationQuotaSpecification(vipQuota);
        return allocationQuotaSpecification;
    }

    private double GetVipQuotaValue(AllocationQuotaCriteria allocationQuotaCriteria)
    {
        var performancePerformanceNature = allocationQuotaCriteria.PerformanceNature;
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