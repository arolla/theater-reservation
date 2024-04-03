using TheaterReservation.Domain;

namespace TheaterReservation.Tests.Domain
{
    [TestClass]
    public class AmountRateArithmeticTest
    {
        [TestMethod]
        public void Apply_discount_rate_to_amount()
        {
            Amount discountOnAmount = new Amount("25.99").Apply(Rate.DiscountPercent("20"));
            Assert.AreEqual(discountOnAmount, new Amount("20.79"));
        }
    }
}
