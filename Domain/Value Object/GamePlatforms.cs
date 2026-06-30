using Domain.Enums;

namespace Domain.Value_Object
{
    public record GamePlatforms
    {
        private readonly List<string> values;
        public IReadOnlyCollection<string> Values => values.AsReadOnly();

        public GamePlatforms(List<EPlatform> platforms)
        {
            if (platforms == null || !platforms.Any())
                throw new ArgumentException("Game must have at least one platform.");

            values = platforms
                .Select(p => p.ToString())
                .Distinct()
                .ToList();
        }
    }
}
