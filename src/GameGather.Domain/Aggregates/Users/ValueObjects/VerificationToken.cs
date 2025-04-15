using GameGather.Domain.Aggregates.Users.Enums;
using GameGather.Domain.Common.Interfaces;
using GameGather.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace GameGather.Domain.Aggregates.Users.ValueObjects;

public sealed class VerificationToken : ValueObject, IToken
{
    public Guid Value { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public TokenType Type { get; private set; }

    [JsonConstructor]
    private VerificationToken()
    {
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(1);
        Type = TokenType.VerificationToken;
    }

    public static VerificationToken Create() => new VerificationToken();

    public VerificationToken Load(
        Guid value,
        DateTime createdOnUtc,
        DateTime expiresOnUtc,
        TokenType type
    )
    {
        Value = value;
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        Type = type;
        return this;
    }
    
    public bool Verify()
    {
        if (ExpiresOnUtc.CompareTo(DateTime.UtcNow) == 1)
        {
            return false;
        }

        return true;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return CreatedOnUtc;
        yield return ExpiresOnUtc;
        yield return Type;
    }
}