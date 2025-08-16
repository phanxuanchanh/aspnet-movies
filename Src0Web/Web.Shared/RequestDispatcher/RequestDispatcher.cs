using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Shared.RequestDispatcher
{
    public class RequestDispatcher
    {
        private readonly Dictionary<Type, object> _handlers = new Dictionary<Type, object>();
        private readonly List<object> _behaviors = new List<object>();

        public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            _handlers[typeof(TRequest)] = handler;
        }

        public void RegisterBehavior<TRequest, TResponse>(IPipelineBehavior<TRequest, TResponse> behavior)
            where TRequest : IRequest<TResponse>
        {
            _behaviors.Add(behavior);
        }

        public TResponse Send<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        {
            Func<TRequest, TResponse> handlerFunc = r =>
            {
                var handler = (IRequestHandler<TRequest, TResponse>)_handlers[typeof(TRequest)];
                return handler.Handle(r);
            };

            // Apply pipeline behaviors
            foreach (var behavior in _behaviors.OfType<IPipelineBehavior<TRequest, TResponse>>().Reverse())
            {
                var next = handlerFunc;
                var b = behavior;
                handlerFunc = r => b.Handle(r, next);
            }

            return handlerFunc(request);
        }
    }
}
