using ErrorOr;
using GameGather.Application.Common.Messaging;
using GameGather.Application.Contracts.Users;
using GameGather.Application.Persistance;
using GameGather.Application.Utils;
using GameGather.Domain.Aggregates.Users;
using GameGather.Domain.Common.Errors;

namespace GameGather.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private IUserRepository _userRepository;
    private IUnitOfWork _unitOfWork;
    private IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var exist = _userRepository.AnyUserAsync(request.Email, cancellationToken);

        if (exist)
        {
            return Errors.User.DuplicateEmail;
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        
        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash,
            request.Birthday);

        _userRepository.AddUserAsync(user);
        
        _unitOfWork.SaveChangesAsync();

        return new RegisterUserResponse(
            user.Id.Value,
            "Pomyslnie zarejestrowano");

    }
}