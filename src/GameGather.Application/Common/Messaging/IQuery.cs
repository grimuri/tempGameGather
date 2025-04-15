using ErrorOr;
using MediatR;

namespace GameGather.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}