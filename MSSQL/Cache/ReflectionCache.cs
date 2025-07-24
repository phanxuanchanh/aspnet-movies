using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace MSSQL.Cache
{
    public static class ReflectionCache
    {
        private static readonly ConcurrentDictionary<Type, object> objectCache
            = new ConcurrentDictionary<Type, object>();

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> propertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetProperties<T>()
        {
            return propertyCache.GetOrAdd(typeof(T), t => t.GetProperties());
        }

        public static T GetObject<T>()
        {
            return (T)objectCache.GetOrAdd(typeof(T), t => Activator.CreateInstance(t));
        }
    }
}
