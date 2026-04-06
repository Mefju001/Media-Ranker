using Domain.Base;
using Domain.DomainService;

namespace Domain.Value_Object;

public record Password:ValueObject
{
    public string HashValue { get; init; }

    private Password(string hashValue)
    {
        HashValue = hashValue;
    }

    public static Password Create(string rawPassword, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException("Password cannot be empty");

        if (rawPassword.Length < 8)
            throw new ArgumentException("Password is too short.");

        var hash = passwordHasher.CreatePasswordHash(rawPassword);
        return new Password(hash);
    }

    public static Password FromHash(string hash) => new Password(hash);

    public static implicit operator string(Password password) => password.HashValue;
    public override string ToString() => "********";
}