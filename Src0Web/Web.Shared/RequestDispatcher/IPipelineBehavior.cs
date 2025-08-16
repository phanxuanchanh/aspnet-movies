using System;

namespace Web.Shared.RequestDispatcher
{
    public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request, Func<TRequest, TResponse> next);
    }
}
