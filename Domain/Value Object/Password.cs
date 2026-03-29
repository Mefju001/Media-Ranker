using Domain.Base;

namespace Domain.Value_Object;

public record Password:ValueObject
{
    public string HashValue { get; init; }

    public Password(string hashValue)
    {
        if (string.IsNullOrWhiteSpace(hashValue))
            throw new ArgumentException("Password hash cannot be empty", nameof(hashValue));

        if (hashValue.Length < 10)
            throw new ArgumentException("Password hash is suspiciously short.");

        HashValue = hashValue;
    }

    public static implicit operator string(Password password) => password.HashValue;

    public static explicit operator Password(string hash) => new(hash);

    public override string ToString() => "********";
}