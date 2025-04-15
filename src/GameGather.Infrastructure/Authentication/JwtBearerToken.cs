using GameGather.Application.Utils;

namespace GameGather.Infrastructure.Authentication;

public record JwtBearerToken(
    string Token,
    DateTime ExpiresOnUtc) : IJwtBearerToken;