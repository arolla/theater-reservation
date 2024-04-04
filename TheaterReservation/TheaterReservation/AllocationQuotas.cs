using TheaterReservation.Data;

namespace TheaterReservation;

public class AllocationQuotas
{
    public double GetVipQuota(Performance performance)
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