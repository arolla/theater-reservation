using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Rate fully()
        {
            return new Rate(ONE);
        }

        public Amount multiply(Amount amount)
        {
            return new Amount(amount.asBigDecimal() * this.value);
        }

        public Rate add(Rate other)
        {
            return new Rate(this.value+other.value);
        }

        public Rate subtract(Rate other)
        {
            return new Rate(this.value-other.value);
        }

        public Decimal asBigDecimal()
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
