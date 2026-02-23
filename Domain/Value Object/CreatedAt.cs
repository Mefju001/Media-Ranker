using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public record CreatedAt
    {
        public DateTime Value { get; private init; }
        public CreatedAt(DateTime value)
        {
            var utcValue = value.ToUniversalTime();

            if (utcValue > DateTime.UtcNow.AddSeconds(5))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The date cannot be in the future.");
            }

            Value = utcValue;
        }
    }
}
