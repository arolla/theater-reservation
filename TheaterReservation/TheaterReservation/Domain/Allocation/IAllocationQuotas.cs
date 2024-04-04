namespace TheaterReservation.Domain.Allocation;

public interface IAllocationQuotas
{
    AllocationQuotaSpecification Find(PerformanceNature performanceNature);
}