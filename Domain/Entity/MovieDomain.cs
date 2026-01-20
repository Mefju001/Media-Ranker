namespace Domain.Entity
{
    public class MovieDomain : MediaDomain
    {
        public int DirectorId { get; private set; }
        public DirectorDomain DirectorDomain {  get; private set; }
        public TimeSpan Duration { get; private  set; }
        public bool IsCinemaRelease { get; private set; } = false;
        private MovieDomain(string Title, string Description, string Language, DateTime ReleaseDate, GenreDomain genre, DirectorDomain director, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, genre)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorDomain = director;
        }
        private MovieDomain(int id, string Title, string Description, string Language, DateTime ReleaseDate, GenreDomain genre, DirectorDomain director, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, genre)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorDomain = director;

        }
        public static MovieDomain Update(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            GenreDomain genreDomain,
            DirectorDomain directorDomain,
            TimeSpan Duration,
            bool IsCinemaRelease, MovieDomain movie)
        {
            Validate(Duration, directorDomain);
            movie.DirectorDomain = directorDomain;
            movie.Duration = Duration;
            movie.IsCinemaRelease = IsCinemaRelease;
            movie.Update(Title, Description, Language!, ReleaseDate, genreDomain);
            return movie;
        }
        public static MovieDomain Create(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            GenreDomain genreDomain,
            DirectorDomain directorDomain,
            TimeSpan Duration,
            bool IsCinemaRelease)
        {
            Validate(Duration, directorDomain);
            return new MovieDomain(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   genreDomain,
                                   directorDomain,
                                   Duration,
                                   IsCinemaRelease);
        }
        public void UpdateCinemaStatus(bool isCinemaRelease)
        {
            IsCinemaRelease = isCinemaRelease;
        }
        private static void Validate(TimeSpan Duration, DirectorDomain Director)
        {
            if (Duration.TotalMinutes <= 0)
                throw new ArgumentException("Duration must be greater than zero.");
            if (Director == null)
                throw new ArgumentException("Director cannot be null.");
        }
        public static MovieDomain Reconstruct(int Id,
            string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            GenreDomain Genre,
            DirectorDomain Director,
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
