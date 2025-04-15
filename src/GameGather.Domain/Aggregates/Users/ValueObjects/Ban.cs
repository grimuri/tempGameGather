using GameGather.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace GameGather.Domain.Aggregates.Users.ValueObjects;

public sealed class Ban : ValueObject
{
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ExpiresOnUtc { get; private set; }
    public string Message { get; private set; }

    [JsonConstructor]
    private Ban(string message, DateTime? expiresOnUtc)
    {
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = expiresOnUtc;
        Message = message;
    }

    public static Ban Create(string message, DateTime? expiresOnUtc) => new Ban(message, expiresOnUtc);

    public Ban Load(
        DateTime createdOnUtc,
        DateTime expiresOnUtc,
        string message
    )
    {
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        Message = message;
        return this;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return CreatedOnUtc;
        yield return ExpiresOnUtc;
        yield return Message;
    }
}