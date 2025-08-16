using System;

namespace Web.Shared.RequestDispatcher
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly Func<TRequest, bool> _validator;

        public ValidationBehavior(Func<TRequest, bool> validator)
        {
            _validator = validator;
        }

        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            if (!_validator(request))
                throw new Exception($"Validation failed for {typeof(TRequest).Name}");

            return next(request);
        }

    }
}
