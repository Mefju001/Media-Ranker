namespace Domain.Value_Object
{
    public record Email
    {
        public string Value { get; init; }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Invalid email format.");
            Value = value;
        }
    }
}
