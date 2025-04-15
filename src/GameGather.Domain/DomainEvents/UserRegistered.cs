using GameGather.Domain.Aggregates.Users;
using GameGather.Domain.Common.Primitives;

namespace GameGather.Domain.DomainEvents;

public sealed record UserRegistered(
    string FirstName,
    string LastName,
    string Email,
    Guid VerificationToken) : IDomainEvent;