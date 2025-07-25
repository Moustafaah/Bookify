using LanguageExt;

using MediatR;

namespace Application.Messaging;
internal interface IQuery<TResponse> : IRequest<Fin<TResponse>>
{
}
