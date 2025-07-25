using MediatR;

namespace Application.Messaging;

internal interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Fin<TResponse>>
    where TCommand : ICommand<TResponse>;
