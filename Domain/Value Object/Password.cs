namespace Domain.Value_Object
{
    public record Password
    {
        public string HashValue { get; init; }
        public Password(string HashValue)
        {
            if (string.IsNullOrEmpty(HashValue)) throw new ArgumentNullException("Password cannot be null");
            this.HashValue = HashValue;
        }
    }
}
