using Domain.Exceptions;

namespace Domain.Value_Object
{
    public record SeasonDetails
    {
        public int Seasons { get; init; }
        public int Episodes { get; init; }

        public SeasonDetails(int seasons, int episodes)
        {
            if (seasons <= 0 || episodes <= 0)
                throw new DomainException("Seasons and Episodes must be greater than zero.");

            if (episodes < seasons)
                throw new DomainException("Total episodes cannot be less than the total number of seasons.");

            Seasons = seasons;
            Episodes = episodes;
        }
    }
}
