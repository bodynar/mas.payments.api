using System;
using System.Collections.Generic;

namespace MAS.Payments.Comparators
{
    public class DatesComparator : IEqualityComparer<DateTime>
    {
        public bool Equals(DateTime x, DateTime y)
        {
            return x.Year == y.Year && x.Month == y.Month;
        }

        public int GetHashCode(DateTime obj)
        {
            return obj.GetHashCode();
        }
    }
}