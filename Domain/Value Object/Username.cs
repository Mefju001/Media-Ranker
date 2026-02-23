using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public record Username
    {
        public string Value { get; init; }
        public Username(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("Username cannot be empty");
            Value = value;
        }
    }
}
