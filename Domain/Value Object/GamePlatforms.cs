using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Value_Object
{
    public record GamePlatforms
    {
        private readonly List<string> values;
        public IReadOnlyCollection<string> Values => values.AsReadOnly();
        private GamePlatforms() 
        {
            values = new List<string>();
        }
        private GamePlatforms(List<EPlatform> platforms)
        {
            if (platforms == null || !platforms.Any())
                throw new DomainException("Game must have at least one platform.");

            values = platforms
                .Select(p => p.ToString())
                .OrderBy(p => p)
                .Distinct()
                .ToList();
        }
        private GamePlatforms(string[] platforms)
        {
            if (platforms == null || !platforms.Any())
                throw new DomainException("Game must have at least one platform.");

            values = platforms
                .Select(p => p)
                .OrderBy(p => p)
                .Distinct()
                .ToList();
        }
        public static GamePlatforms FromPlatforms(List<EPlatform> platforms)
        {
            return DeterminePlayablePlatforms(platforms);
        }
        public static GamePlatforms FromStrings(string[] platforms)
        {
            return new GamePlatforms(platforms);
        }
        private static GamePlatforms DeterminePlayablePlatforms(List<EPlatform> platforms)
        {
            if (platforms == null || !platforms.Any())
                throw new DomainException("Game must have at least one platform.");

            var all = new HashSet<EPlatform>();
            foreach (var p in platforms)
            {
                all.Add(p);

                if (p == EPlatform.PlayStation4) all.Add(EPlatform.PlayStation5);
                if (p == EPlatform.XboxOne) all.Add(EPlatform.XboxSeries);
            }

            return new GamePlatforms(all.ToList());
        }
        public virtual bool Equals(GamePlatforms? obj)
        {
            if(obj is null)
                return false;
            if (ReferenceEquals(this, obj)) return true;
            if (Values.Count != obj.Values.Count) return false;
            return values.SequenceEqual(obj.values);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var item in values)
            {
                hash.Add(item);
            }
            return hash.ToHashCode();
        }
    }
}
