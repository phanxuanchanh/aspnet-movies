using System;
using System.Collections.Concurrent;

namespace MSSQL.QueryBuilder
{
    internal static class SqlQueryCache
    {
        private static readonly ConcurrentDictionary<(Type, string), string> _selectStatementCache
            = new ConcurrentDictionary<(Type, string), string>();

        public static readonly ConcurrentDictionary<(Type, string), string> UpdateStatementCache
            = new ConcurrentDictionary<(Type, string), string>();

        public static readonly ConcurrentDictionary<Type, string> InsertStatementCache
            = new ConcurrentDictionary<Type, string>();

        public static readonly ConcurrentDictionary<(Type, string), string> OrderByClauseCache
            = new ConcurrentDictionary<(Type, string), string>();

        private static readonly ConcurrentDictionary<(string, string, string), string> _sqlStatementCache
            = new ConcurrentDictionary<(string, string, string), string>();


        public static bool TryGetSelectStatement((Type, string) key, out string statement)
        {
            return _selectStatementCache.TryGetValue(key, out statement);
        }

        public static void AddOrUpdateSelectStatement((Type, string) key, string statement)
        {
            _selectStatementCache.AddOrUpdate(key, statement, (k, oldvalue) => statement);
        }

        public static bool TryGetSqlStatement((string, string, string) key, out string statement)
        {
            return _sqlStatementCache.TryGetValue(key, out statement);
        }

        public static void AddOrUpdateSqlStatement((string, string, string) key, string statement)
        {
            _sqlStatementCache.AddOrUpdate(key, statement, (k, oldvalue) => statement);
        }
    }
}
