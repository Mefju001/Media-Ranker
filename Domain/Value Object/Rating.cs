namespace Domain.Value_Object
{
    public record Rating
    {
        public int value { get; init; }
        public Rating(int value)
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 10.");
            this.value = value;
        }
    }
}
