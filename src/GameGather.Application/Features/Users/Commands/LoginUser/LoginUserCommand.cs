using GameGather.Application.Common.Messaging;
using GameGather.Application.Contracts.Users;

namespace GameGather.Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginUserResponse>;