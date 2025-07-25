using LanguageExt;

using MediatR;

namespace Application.Messaging;
internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Fin<TResponse>> where TQuery : IQuery<TResponse>
{
}
