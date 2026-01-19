namespace Domain.Entity
{
    public class MovieDomain : MediaDomain
    {
        public int DirectorId { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsCinemaRelease { get; set; } = false;
        private MovieDomain(string Title, string Description, string Language, DateTime ReleaseDate, int GenreId, int DirectorId, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, GenreId)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = DirectorId;
        }
        private MovieDomain(int id, string Title, string Description, string Language, DateTime ReleaseDate, int GenreId, int DirectorId, TimeSpan Duration, bool IsCinemaRelease)
            : base(Title, Description, Language, ReleaseDate, GenreId)
        {
            this.Duration = Duration;
            this.IsCinemaRelease = IsCinemaRelease;
            this.DirectorId = DirectorId;

        }
        public static MovieDomain Update(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int GenreId,
            int DirectorId,
            TimeSpan Duration,
            bool IsCinemaRelease, MovieDomain movie)
        {
            Validate(Duration,DirectorId);
            movie.DirectorId = DirectorId;
            movie.Duration = Duration;
            movie.IsCinemaRelease = IsCinemaRelease;
            movie.Update(Title, Description, Language!, ReleaseDate, GenreId);
            return movie;
        }
        public static MovieDomain Create(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int GenreId,
            int DirectorId,
            TimeSpan Duration,
            bool IsCinemaRelease)
        {
            Validate(Duration, DirectorId);
            return new MovieDomain(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   GenreId,
                                   DirectorId,
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
            if (Director >0)
                throw new ArgumentException("Director id cannot be null.");
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
