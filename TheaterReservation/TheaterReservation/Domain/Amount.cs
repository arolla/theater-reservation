using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterReservation.Domain
{
    public class Amount
    {
        private const Decimal ZERO = 0.0M;
        private readonly Decimal value;

        public Amount(Decimal value)
        {
            this.value = Math.Round(value, 2, MidpointRounding.ToEven);
        }

        public Amount(Amount other)
        {
            this.value = ZERO + other.value;
        }

        public Amount multiply(Rate rate)
        {
            return new Amount(this.value * rate.asBigDecimal());
        }

        public static Amount nothing()
        {
            return new Amount(ZERO);
        }

        public Amount add(Amount other)
        {
            return new Amount(this.value + other.value);
        }

        public Decimal asBigDecimal()
        {
            return value;
        }

        public String asString()
        {
            return value.ToString();
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
