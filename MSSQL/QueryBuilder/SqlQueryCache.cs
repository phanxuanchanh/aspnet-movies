using System;
using System.Collections.Concurrent;

namespace MSSQL.QueryBuilder
{
    internal static class SqlQueryCache
    {
        public static readonly ConcurrentDictionary<(Type, string), string> SelectStatementCache
            = new ConcurrentDictionary<(Type, string), string>();

        public static readonly ConcurrentDictionary<(Type, string), string> UpdateStatementCache
            = new ConcurrentDictionary<(Type, string), string>();

        public static readonly ConcurrentDictionary<Type, string> InsertStatementCache
            = new ConcurrentDictionary<Type, string>();

        public static readonly ConcurrentDictionary<(Type, string), string> OrderByClauseCache
            = new ConcurrentDictionary<(Type, string), string>();
    }
}
