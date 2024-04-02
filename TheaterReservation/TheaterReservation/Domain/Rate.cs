namespace TheaterReservation.Domain
{
    public class Rate
    {
        private const Decimal ZERO = 0.0M;
        private const Decimal ONE = 1.0M;
        private readonly Decimal value;

        public Rate(String value)
        {
            this.value = Math.Round(Convert.ToDecimal(value), 3, MidpointRounding.ToEven);
        }

        public Rate(Decimal value)
        {
            this.value = value;
        }

        public Rate(Rate other)
        {
            this.value = ZERO + other.value;
        }

        public static Rate Fully()
        {
            return new Rate(ONE);
        }

        public Amount Multiply(Amount amount)
        {
            return new Amount(amount.AsBigDecimal() * this.value);
        }

        public Rate Add(Rate other)
        {
            return new Rate(this.value+other.value);
        }

        public Rate Subtract(Rate other)
        {
            return new Rate(this.value-other.value);
        }

        public Decimal AsBigDecimal()
        {
            return value;
        }
        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            Rate? rate = obj as Rate;
            return rate != null && value.Equals(rate.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
