using TheaterReservation.Domain.Allocation;

namespace TheaterReservation.Tests.Domain
{
    [TestClass]
    public class AllocationQuotaSpecificationTest
    {
        [DataRow(15, 40, 0.5, true)]
        [DataRow(25, 40, 0.5, false)]
        [DataRow(19, 40, 0.5, true)]
        [DataRow(21, 40, 0.5, false)]
        [DataRow(20, 40, 0.5, false)]
        [DataRow(5, 40, -1, false)]
        [DataTestMethod]

        public void allocation_quota_specification(int availableSeatsCount, int totalSeatsCount, double shelvingQuota, bool expected)
        {
            AllocationQuotaSpecification allocationQuotaSpecification = new AllocationQuotaSpecification(shelvingQuota);
            bool satisfiedBy = allocationQuotaSpecification.IsSatisfiedBy(
                new PerformanceInventory(availableSeatsCount, totalSeatsCount));

            Assert.AreEqual(expected, satisfiedBy);
        }
    }
}
