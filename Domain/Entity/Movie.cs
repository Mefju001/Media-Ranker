using Domain.Value_Object;

namespace Domain.Entity
{
    public class Movie : Media
    {
        public int DirectorId { get; private set; }
        public Duration Duration { get; private set; }
        public bool IsCinemaRelease { get; private set; } = false;
        private Movie()
        {
        }
        private Movie(string Title, string Description, Language Language, ReleaseDate ReleaseDate, int genreId, int directorId, Duration Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, genreId)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = directorId;
        }
        private Movie(int id, string Title, string Description, Language Language, ReleaseDate ReleaseDate, int genre, int director, Duration Duration, bool IsCinemaRelease, MediaStats stats)
            : base(id, Title, Description, Language, ReleaseDate, genre, stats)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = director;

        }
        public void Update(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genreDomain,
            int directorDomain,
            Duration Duration,
            bool IsCinemaRelease)
        {
            Validate(directorDomain);
            this.DirectorId = directorDomain;
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            base.Update(Title, Description, Language, ReleaseDate, genreDomain);
        }
        public static Movie Create(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genreDomain,
            int director,
            Duration Duration,
            bool IsCinemaRelease)
        {
            Validate(director);
            return new Movie(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   genreDomain,
                                   director,
                                   Duration,
                                   IsCinemaRelease);
        }
        public void UpdateCinemaStatus(bool isCinemaRelease)
        {
            IsCinemaRelease = isCinemaRelease;
        }
        private static void Validate(int Director)
        {
            if (Director <= 0)
                throw new ArgumentException("Director must be greater than zero.");
        }
        public static Movie Reconstruct(int Id,
            string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int Genre,
            int Director,
            Duration Duration,
            bool IsCinemaRelease,
            MediaStats stats)
        {
            return new Movie(Id,
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Director,
                                   Duration,
                                   IsCinemaRelease,
                                   stats);
        }
    }
}
