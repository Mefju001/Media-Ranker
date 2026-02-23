using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class Fullname
    {
        public string Name { get; init; }
        public string Surname { get; init; }

        public Fullname(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname cannot be empty.");

            Name = name;
            Surname = surname;
        }

        
        public override string ToString() => $"{Name} {Surname}";
    }
}
