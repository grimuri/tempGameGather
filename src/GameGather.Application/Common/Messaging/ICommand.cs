using ErrorOr;
using MediatR;

namespace GameGather.Application.Common.Messaging;

public interface ICommand : IRequest<ErrorOr<Unit>>
{
    
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}