namespace TheaterReservation.Domain.Allocation;

public interface IAllocationQuotas
{
    AllocationQuotaSpecification GetVipQuota(AllocationQuotaCriteria allocationQuotaCriteria);
}