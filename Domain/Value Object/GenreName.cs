namespace Domain.Value_Object
{
    public record GenreName
    {
        public string Value { get; init; }
        public GenreName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Genre name cannot be empty");
            }
            this.Value = value;
        }
        public override string ToString() => Value;
    }
}
