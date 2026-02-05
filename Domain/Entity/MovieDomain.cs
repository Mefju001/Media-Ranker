namespace Domain.Entity
{
    public class MovieDomain : MediaDomain
    {
        public int DirectorId { get; private set; }
        public TimeSpan Duration { get; private  set; }
        public bool IsCinemaRelease { get; private set; } = false;
        private MovieDomain()
        {
        }
        private MovieDomain(string Title, string Description, string Language, DateTime ReleaseDate, int genreId, int directorId, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, genreId)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = directorId;
        }
        private MovieDomain(int id, string Title, string Description, string Language, DateTime ReleaseDate, int genre, int director, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, genre)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = director;

        }
        public static MovieDomain Update(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int genreDomain,
            int directorDomain,
            TimeSpan Duration,
            bool IsCinemaRelease, MovieDomain movie)
        {
            Validate(Duration, directorDomain);
            movie.DirectorId = directorDomain;
            movie.Duration = Duration;
            movie.IsCinemaRelease = IsCinemaRelease;
            movie.Update(Title, Description, Language!, ReleaseDate, genreDomain);
            return movie;
        }
        public static MovieDomain Create(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int genreDomain,
            int director,
            TimeSpan Duration,
            bool IsCinemaRelease)
        {
            Validate(Duration, director);
            return new MovieDomain(
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
        private static void Validate(TimeSpan Duration, int Director)
        {
            if (Duration.TotalMinutes <= 0)
                throw new ArgumentException("Duration must be greater than zero.");
            if (Director<=0)
                throw new ArgumentException("Director must be greater than zero.");
        }
        public static MovieDomain Reconstruct(int Id,
            string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int Genre,
            int Director,
            TimeSpan Duration,
            bool IsCinemaRelease)
        {
            return new MovieDomain(Id,
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Director,
                                   Duration,
                                   IsCinemaRelease);
        }
    }
}
