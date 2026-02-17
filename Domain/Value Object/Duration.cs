using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public record Duration
    {
        public TimeSpan Value { get; init; }
        public Duration(TimeSpan value)
        {
            if (value.Minutes <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration must be a positive integer.");
            if(value.Minutes > 720)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration must be less than or equal to 12 hours.");
            Value = value;
        }
    }
}
