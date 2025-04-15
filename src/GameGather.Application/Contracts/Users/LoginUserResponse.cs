namespace GameGather.Application.Contracts.Users;

public record LoginUserResponse(
    int Id,
    string Firstname,
    string Lastname,
    string Email,
    string Token,
    DateTime TokenExpiresOnUtc,
    bool PasswordExpired = false);