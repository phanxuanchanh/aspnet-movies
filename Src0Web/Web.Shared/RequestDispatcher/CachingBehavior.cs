using System;
using System.Web;

namespace Web.Shared.RequestDispatcher
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        //private readonly Cache _cache = HttpRuntime.Cache;
        private readonly int _durationSeconds;

        public CachingBehavior(int durationSeconds = 60)
        {
            _durationSeconds = durationSeconds;
        }

        public TResponse Handle(TRequest request, Func<TRequest, TResponse> next)
        {
            //string key = $"CACHE_{typeof(TRequest).FullName}_{request.GetHashCode()}";
            //var cached = (TResponse)_cache[key];
            //if (cached != null) return cached;

            var response = next(request);
            //_cache.Insert(key, response, null, DateTime.Now.AddSeconds(_durationSeconds), Cache.NoSlidingExpiration);
            return response;
        }
    }
}
