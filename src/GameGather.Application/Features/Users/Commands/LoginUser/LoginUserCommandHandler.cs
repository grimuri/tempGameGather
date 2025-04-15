using ErrorOr;
using GameGather.Application.Common.Messaging;
using GameGather.Application.Configurations;
using GameGather.Application.Contracts.Users;
using GameGather.Application.Persistance;
using GameGather.Application.Utils;
using GameGather.Domain.Common.Errors;
using Microsoft.Extensions.Options;

namespace GameGather.Application.Features.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly PasswordOptions _passwordOptions;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork, 
        IJwtProvider jwtProvider, 
        IPasswordHasher passwordHasher, 
        IOptions<PasswordOptions> passwordOptions)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _passwordOptions = passwordOptions.Value;
    }

    public async Task<ErrorOr<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        // User not found
        if (user is null)
        {
            return Errors.User.InvalidCredentials;
        }

        // Wrong password
        if (!_passwordHasher.Verify(request.Password, user.Password.Value))
        {
            return Errors.User.InvalidCredentials;
        }
        
        // Password expired
        if (user.Password.IsExpired(_passwordOptions.ExpiryInDays))
        {
            return new LoginUserResponse(
                user.Id.Value,
                user.FirstName,
                user.LastName,
                user.Email,
                default,
                default,
                true);
        }
        
        var token = _jwtProvider.GenerateToken(user);

        return new LoginUserResponse(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Email,
            token.Token,
            token.ExpiresOnUtc);

    }
}