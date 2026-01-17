using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class TvSeriesDomain : MediaDomain
    {
        public int Seasons { get; private set; }
        public int Episodes { get; private set; }
        public string? Network { get; private set; }
        public EStatus Status { get; private set; }
        public TvSeriesDomain(string Title, 
            string Description, 
            string Language, 
            DateTime ReleaseDate, 
            int GenreId,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status) 
            : base(Title, Description, Language, ReleaseDate, GenreId)
        {
            this.Seasons = Seasons;
            this.Episodes = Episodes;
            this.Network = Network;
            this.Status = Status;
        }
        public static TvSeriesDomain Create(string Title,
            string Description,
            string Language,
            DateTime ReleaseDate,
            int GenreId,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status)
        {
            Validate(Seasons, Episodes);
            return new TvSeriesDomain(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   GenreId,
                                   Seasons,
                                   Episodes,
                                   Network,
                                   Status);
        }
        public void UpdateSeasons(int seasons)
        {
            if (seasons <= 0)
                throw new ArgumentException("Seasons must be greater than zero.");
            Seasons = seasons;
        }
        public void UpdateEpisodes(int episodes)
        {
            if (episodes <= 0)
                throw new ArgumentException("Episodes must be greater than zero.");
            Episodes = episodes;
        }
        public void UpdateNetwork(string network)
        {
            Network = network;
        }
        public void UpdateStatus(EStatus status)
        {
            Status = status;
        }
        private static void Validate(int Seasons, int Episodes)
        {
            if (Seasons <= 0)
                throw new ArgumentException("Seasons must be greater than zero.");
            if (Episodes <= 0)
                throw new ArgumentException("Episodes must be greater than zero.");
        }
    }
}
