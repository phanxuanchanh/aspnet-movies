using System;

namespace MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlColumnAttribute : Attribute
    {
        public string ColumnName { get; }
        public bool PrimaryKey { get; set; }
        public bool AutoIncrement { get; set; }

        public SqlColumnAttribute(string columnName, bool primarykey = false, bool autoIncrement = false)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            ColumnName = columnName;
            PrimaryKey = primarykey;
            AutoIncrement = autoIncrement;
        }
    }
}