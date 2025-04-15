using GameGather.Domain.Aggregates.Users;

namespace GameGather.Application.Utils;

public interface IJwtProvider
{
    IJwtBearerToken GenerateToken(User user);
}