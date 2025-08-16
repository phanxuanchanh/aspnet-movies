using System;
using System.Reflection;

namespace Web.Shared.Mapper
{
    public static class AutoMapperHelper
    {
        public static Func<TSource, TTarget> AutoMap<TSource, TTarget>()
            where TTarget : new()
        {
            PropertyInfo[] sourceProps = typeof(TSource).GetProperties();
            PropertyInfo[] targetProps = typeof(TTarget).GetProperties();

            return source =>
            {
                TTarget target = new TTarget();
                foreach (var tp in targetProps)
                {
                    var sp = Array.Find(sourceProps, p => p.Name == tp.Name && p.PropertyType == tp.PropertyType);
                    if (sp != null)
                    {
                        tp.SetValue(target, sp.GetValue(source));
                    }
                }
                return target;
            };
        }
    }
}
