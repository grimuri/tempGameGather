using GameGather.Domain.Aggregates.Users.Enums;

namespace GameGather.Domain.Common.Interfaces;

public interface IToken
{
    bool Verify();
}