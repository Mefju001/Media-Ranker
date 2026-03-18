namespace Domain.Value_Object
{
    public record Duration
    {
        public TimeSpan Value { get; init; }
        private Duration()
        { }
        public Duration(TimeSpan value)
        {
            if (value.TotalMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration must be a positive integer.");
            if (value.TotalMinutes > 720)
                throw new ArgumentOutOfRangeException(nameof(value), "Duration must be less than or equal to 12 hours.");
            Value = value;
        }
    }
}
