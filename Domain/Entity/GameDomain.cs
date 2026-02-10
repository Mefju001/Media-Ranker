using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class GameDomain: MediaDomain
    {
        public string Developer { get; private set; }
        public EPlatform Platform { get; private set; }
        private GameDomain() { }
        private GameDomain(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int genreId,
            string Developer,
            EPlatform Platform)
            : base(Title, Description, Language, ReleaseDate, genreId)
        {
            this.Developer = Developer;
            this.Platform = Platform;
        }
        private GameDomain(int Id, string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int genreId,
            string Developer,
            EPlatform Platform)
            : base(Title, Description, Language, ReleaseDate, genreId)
        {
            this.Developer = Developer;
            this.Platform = Platform;
            this.Id = Id;
        }
        public static GameDomain Create(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform)
        {
            Validate(Developer);
            return new GameDomain(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Developer,
                                   Platform);
        }
        public static GameDomain Reconstruct(int Id, string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform)
        {
            Validate(Developer);
            var game = new GameDomain(Id,
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   Genre,
                                   Developer,
                                   Platform);
            return game;
        }
        public static GameDomain Update(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int Genre,
            string Developer,
            EPlatform Platform, GameDomain game)
        {
            Validate(Developer);
            game.Developer = Developer;
            game.Platform = Platform;
            game.Update(Title, Description, Language, ReleaseDate, Genre);
            return game;
        }
        private static void Validate(string Developer)
        {
            if (string.IsNullOrWhiteSpace(Developer)) throw new ArgumentException("Developer cannot be null or empty.");
        }
    }
}
