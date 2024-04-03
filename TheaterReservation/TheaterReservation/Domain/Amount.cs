using System.Globalization;

namespace TheaterReservation.Domain
{
    public class Amount
    {
        private const decimal ZERO = 0.0M;
        private readonly decimal value;

        public Amount(decimal value)
        {
            this.value = Math.Round(value, 2, MidpointRounding.ToEven);
        }

        public Amount(string value)
        {
            this.value = Math.Round(Convert.ToDecimal(value), 2, MidpointRounding.ToEven);
        }

        public Amount(Amount other)
        {
            this.value = ZERO + other.value;
        }

        public Amount Apply(Rate rate)
        {
            return new Amount(this.value * rate.AsBigDecimal());
        }

        public static Amount Nothing()
        {
            return new Amount(ZERO);
        }

        public Amount Add(Amount other)
        {
            return new Amount(this.value + other.value);
        }

        public decimal AsBigDecimal()
        {
            return value;
        }

        public string AsString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            Amount? amount = obj as Amount;
            return amount != null && value.Equals(amount.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
