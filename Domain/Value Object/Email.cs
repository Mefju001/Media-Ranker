
namespace Domain.Value_Object
{
    public record Email
    {
        private readonly string value;
        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));
            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format", nameof(value));
            this.value = value;
        }
        public static Email Create(string value)
        {
            return new Email(value);
        }
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public override string ToString()
        {
            return value;
        }
    }
}
