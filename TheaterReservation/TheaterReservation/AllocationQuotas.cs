using TheaterReservation.Data;
using TheaterReservation.Domain.Allocation;

namespace TheaterReservation;

public class AllocationQuotas
{
    public AllocationQuotaSpecification GetVipQuotaSpecification(Performance performance)
    {
        var vipQuota = GetVipQuota(performance);
        var allocationQuotaSpecification = new AllocationQuotaSpecification(vipQuota);
        return allocationQuotaSpecification;
    }

    private double GetVipQuota(Performance performance)
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