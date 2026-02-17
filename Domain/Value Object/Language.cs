using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public record Language
    {
        public string value { get; init; }
        public Language(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Language cannot be null or empty.", nameof(value));
            this.value = value;
        }
    }
}
