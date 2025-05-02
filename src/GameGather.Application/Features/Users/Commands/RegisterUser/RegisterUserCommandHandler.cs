using System.Diagnostics;
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
        var stopwatch = Stopwatch.StartNew();
        
        var exist = await _userRepository.AnyUserAsync(request.Email, cancellationToken);

        stopwatch.Stop();
        Console.WriteLine($"Checking if user exists took: {stopwatch.ElapsedMilliseconds} ms");
        
        stopwatch.Restart();
        
        if (exist)
        {
            return Errors.User.DuplicateEmail;
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        
        stopwatch.Stop();
        Console.WriteLine($"Hashing password took: {stopwatch.ElapsedMilliseconds} ms");
        
        stopwatch.Restart();
        
        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash,
            request.Birthday);
        
        stopwatch.Stop();
        Console.WriteLine($"Creating user took: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();

        await _userRepository.AddUserAsync(user, cancellationToken);
        
        stopwatch.Stop();
        Console.WriteLine($"Adding user to repository took: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        stopwatch.Stop();
        Console.WriteLine($"Saving changes took: {stopwatch.ElapsedMilliseconds} ms");

        return new RegisterUserResponse(
            user.Id.Value,
            "Pomyslnie zarejestrowano");

    }
}