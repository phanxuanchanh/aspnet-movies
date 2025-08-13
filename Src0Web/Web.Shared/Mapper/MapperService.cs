using System;
using System.Collections.Generic;

namespace Web.Shared.Mapper
{
    public class MapperService
    {
        private readonly Dictionary<(Type, Type), object> _handlers = new Dictionary<(Type, Type), object>();

        public void Add<TSource, TDest>(IMappingProfile<TSource, TDest> handler)
        {
            var key = (typeof(TSource), typeof(TDest));
            _handlers[key] = handler;
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            var key = (typeof(TSource), typeof(TDest));
            if (_handlers.TryGetValue(key, out var handlerObj) && handlerObj is IMappingProfile<TSource, TDest> handler)
            {
                return handler.Map(source);
            }
            throw new InvalidOperationException($"No mapper registered for {typeof(TSource).Name} -> {typeof(TDest).Name}");
        }
    }
}
