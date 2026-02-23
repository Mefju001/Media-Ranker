using Domain.Enums;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class TvSeries : Media
    {
        public int Seasons { get; private set; }
        public int Episodes { get; private set; }
        public string? Network { get; private set; }
        public EStatus Status { get; private set; }
        private TvSeries() { }
        private TvSeries(string Title, 
            string Description, 
            Language Language, 
            ReleaseDate ReleaseDate,
            int genre,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status) 
            : base(Title, Description, Language, ReleaseDate, genre)
        {
            this.Seasons = Seasons;
            this.Episodes = Episodes;
            this.Network = Network;
            this.Status = Status;
        }
        private TvSeries(int id,
            string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genre,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status,
            MediaStats stats)
            : base(id,Title, Description, Language, ReleaseDate, genre, stats)
        {
            this.Seasons = Seasons;
            this.Episodes = Episodes;
            this.Network = Network;
            this.Status = Status;
        }
        public static TvSeries Create(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genre,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status)
        {
            Validate(Seasons, Episodes);
            return new TvSeries(
                                   Title,
                                   Description,
                                   Language,
                                   ReleaseDate,
                                   genre,
                                   Seasons,
                                   Episodes,
                                   Network,
                                   Status);
        }
        public void Update(string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genre,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status)
        {
            Validate(Seasons, Episodes);
            this.Seasons = Seasons;
            this.Episodes = Episodes;
            this.Network = Network;
            this.Status = Status;
            base.Update(Title, Description,Language,ReleaseDate,genre);            
        }
        public static TvSeries Reconstruct(int id,
            string Title,
            string Description,
            Language Language,
            ReleaseDate ReleaseDate,
            int genre,
            int Seasons,
            int Episodes,
            string? Network,
            EStatus Status,
            MediaStats stats)
        {
            return new TvSeries(id,
                                Title,
                                Description,
                                Language,
                                ReleaseDate,
                                genre,
                                Seasons,
                                Episodes,
                                Network,
                                Status,
                                stats);
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
