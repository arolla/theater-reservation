using TheaterReservation.Data;
using TheaterReservation.Domain.Allocation;

namespace TheaterReservation;

public interface IAllocationQuotas
{
    AllocationQuotaSpecification GetVipQuota(Performance performance);
}

public class AllocationQuotas : IAllocationQuotas
{
    public AllocationQuotaSpecification GetVipQuota(Performance performance)
    {
        var vipQuota = GetVipQuotaValue(performance);
        var allocationQuotaSpecification = new AllocationQuotaSpecification(vipQuota);
        return allocationQuotaSpecification;
    }

    private double GetVipQuotaValue(Performance performance)
    {
        switch (performance.performanceNature)
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