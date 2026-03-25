using Domain.Enums;
using Domain.Value_Object;

namespace Domain.Entity
{
    public class Game : Media
    {
        public string Developer { get; private set; }
        public EPlatform Platform { get; private set; }
        private Game() { }
        private Game(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genreId,
            string Developer,
            EPlatform Platform)
            : base(Title, Description, Language, ReleaseDate, genreId)
        {
            this.Developer = Developer;
            this.Platform = Platform;
        }
        private Game(int Id, string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genreId,
            string Developer,
            EPlatform Platform, MediaStats stats)
            : base(Id, Title, Description, Language, ReleaseDate, genreId, stats)
        {
            this.Developer = Developer;
            this.Platform = Platform;
        }
        public static Game Create(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform)
        {
            Validate(Developer);
            return new Game(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Developer,
                                   Platform);
        }
        public static Game Reconstruct(int Id, string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform,
            MediaStats stats)
        {
            Validate(Developer);
            var game = new Game(Id,
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Developer,
                                   Platform,
                                   stats);
            return game;
        }
        public void Update(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform)
        {
            Validate(Developer);
            this.Developer = Developer;
            this.Platform = Platform;
            base.Update(Title, Description, Language, ReleaseDate, Genre);
        }
        private static void Validate(string Developer)
        {
            if (string.IsNullOrWhiteSpace(Developer)) throw new ArgumentException("Developer cannot be null or empty.");
        }
    }
}
