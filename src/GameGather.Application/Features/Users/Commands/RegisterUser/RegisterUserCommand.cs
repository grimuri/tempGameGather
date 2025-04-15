using GameGather.Application.Common.Messaging;
using GameGather.Application.Contracts.Users;

namespace GameGather.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    DateTime Birthday) : ICommand<RegisterUserResponse>;