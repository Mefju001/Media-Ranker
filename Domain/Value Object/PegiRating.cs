using Domain.Exceptions;

namespace Domain.Value_Object
{
    public class PegiRating
    {
        public static PegiRating Pegi3 => new(3, "PEGI 3 - Odpowiednie dla wszystkich grup wiekowych.");
        public static PegiRating Pegi7 => new(7, "PEGI 7 - Odpowiednie dla starszych dzieci.");
        public static PegiRating Pegi12 => new(12, "PEGI 12 - Przeznaczone dla osób od 12 roku życia.");
        public static PegiRating Pegi16 => new(16, "PEGI 16 - Przeznaczone dla młodzieży od 16 roku życia.");
        public static PegiRating Pegi18 => new(18, "PEGI 18 - Gra tylko dla dorosłych.");

        private static readonly List<PegiRating> All = new() { Pegi3, Pegi7, Pegi12, Pegi16, Pegi18 };

        public int AgeValue { get; private set; }
        public string Description { get; private set; }

        private PegiRating(int ageValue, string description)
        {
            AgeValue = ageValue;
            Description = description;
        }

        private PegiRating() { }

        public static PegiRating FromValue(int value)
        {
            var rating = All.FirstOrDefault(p => p.AgeValue == value);
            if (rating == null)
                throw new DomainException($"Invalid PEGI rating value: {value}.");

            return rating;
        }
    }
}
