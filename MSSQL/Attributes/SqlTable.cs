using System;

namespace MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlTableAttribute : Attribute
    {
        public string TableName { get; }

        public SqlTableAttribute(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            TableName = tableName;
        }
    }
}
