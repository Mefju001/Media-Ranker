namespace Domain.Value_Object
{
    public record GameDetails
    {
        public string Developer { get; init; }
        public string? Engine { get; init; }
        public GameDetails(string developer, string? engine)
        {
            if (string.IsNullOrWhiteSpace(developer))
                throw new ArgumentException("Developer cannot be null or empty.");

            Developer = developer.Trim();
            Engine = string.IsNullOrWhiteSpace(engine) ? null : engine.Trim();
            
        }
    }
}
