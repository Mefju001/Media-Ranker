namespace Domain.Value_Object
{
    public record ReleaseDate
    {
        public DateTime Value { get; init; }
        public ReleaseDate(DateTime value)
        {
            if (value > DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(value), "Release date cannot be in the future.");
            Value = value;
        }
    }
}
