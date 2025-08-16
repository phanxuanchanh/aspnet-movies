using System;

namespace Web.Shared.RequestDispatcher
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            TResponse response = next(request);
            return response;
        }
    }
}