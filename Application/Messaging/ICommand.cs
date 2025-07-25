using MediatR;

namespace Application.Messaging;

internal interface ICommand<TResponse> : IRequest<Fin<TResponse>>
{
}
