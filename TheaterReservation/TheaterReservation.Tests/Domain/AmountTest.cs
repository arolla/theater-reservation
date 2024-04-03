using TheaterReservation.Domain;

namespace TheaterReservation.Tests.Domain;

[TestClass]
public class AmountTest
{
    [TestMethod]
    public void Add_two_amounts()
    {
        Amount amountAddition = new Amount("25.99").Add(new Amount("59.99"));
        Assert.AreEqual(amountAddition, new Amount("85.98"));
    }

    [TestMethod]
    public void Amount_nothing_is_addition_neutral_element()
    {
        Amount amount = new Amount("25.99");
        Amount amountAddition = amount.Add(Amount.Nothing());
        Assert.AreEqual(amountAddition, amount);
    }

}