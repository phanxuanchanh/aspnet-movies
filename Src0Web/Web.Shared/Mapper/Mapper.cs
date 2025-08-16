using System;
using System.Collections.Generic;

namespace Web.Shared.Mapper
{
    public class Mapper : IMapper
    {
        private readonly Dictionary<(Type Source, Type Target), Delegate> _mappings
            = new Dictionary<(Type, Type), Delegate>();

        public void CreateMap<TSource, TTarget>(Func<TSource, TTarget> mapFunc)
        {
            _mappings[(typeof(TSource), typeof(TTarget))] = mapFunc;
        }

        public void CreateMap<TSource, TTarget, TMapper>()
        where TMapper : ITypeMapper<TSource, TTarget>, new()
        {
            TMapper mapperInstance = new TMapper();
            _mappings[(typeof(TSource), typeof(TTarget))] =
                new Func<TSource, TTarget>(mapperInstance.Map);
        }

        public TTarget Map<TSource, TTarget>(TSource source)
        {
            if (_mappings.TryGetValue((typeof(TSource), typeof(TTarget)), out var func))
            {
                return ((Func<TSource, TTarget>)func)(source);
            }
            throw new InvalidOperationException($"No mapping from {typeof(TSource)} to {typeof(TTarget)}");
        }

        public void AddProfile(IMappingProfile profile)
        {
            profile.Configure(this);
        }
    }
}
