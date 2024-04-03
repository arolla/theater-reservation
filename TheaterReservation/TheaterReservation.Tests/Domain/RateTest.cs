using TheaterReservation.Domain;

namespace TheaterReservation.Tests.Domain
{
    [TestClass]
    public class RateTest
    {
        [TestMethod]
        public void Add_two_rates()
        {
            Rate rateAddition = new Rate("0.15").Add(new Rate("0.2"));
            Assert.AreEqual(rateAddition, new Rate("0.35"));
        }

        [TestMethod]
        public void Discount_percent()
        {
            Rate discountPercent = Rate.DiscountPercent("25");
            Assert.AreEqual(discountPercent, new Rate("0.75"));
        }
    }
}
