using MSSQL.Attributes;
using MSSQL.Cache;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MSSQL.Mapper
{
    public class SqlMapper
    {
        private static void SetValue<T>(T instance, PropertyInfo tProperty, object rawValue)
        {
            Type targetType = Nullable.GetUnderlyingType(tProperty.PropertyType) ?? tProperty.PropertyType;
            object value = (rawValue == null) ? null : Convert.ChangeType(rawValue, targetType);
            tProperty.SetValue(instance, value);
        }

        /***
         * Map a row from a SqlDataReader to an object of type T
         * 
         * @param reader SqlDataReader
         * @return T
         */
        public static T MapRow<T>(SqlDataReader reader) where T : ISqlTable, new()
        {
            T instance = new T();
            PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();

            foreach (PropertyInfo tProperty in tProperties)
            {
                SqlColumnAttribute attribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
                string columnName = attribute is null ? tProperty.Name : attribute.ColumnName;

                if (reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                    SetValue(instance, tProperty, reader.GetValue(reader.GetOrdinal(columnName)));
            }

            return instance;
        }

        public static T MapRow<T>(DataRow row) where T : ISqlTable, new()
        {
            T instance = new T();
            PropertyInfo[] tProperties = ReflectionCache.GetProperties<T>();

            foreach (PropertyInfo tProperty in tProperties)
            {
                SqlColumnAttribute attribute = tProperty.GetCustomAttribute<SqlColumnAttribute>();
                string columnName = attribute is null ? tProperty.Name : attribute.ColumnName;
                if (row.Table.Columns.Contains(columnName) && !row.IsNull(columnName))
                    SetValue(instance, tProperty, row[columnName]);
            }
            return instance;
        }
    }
}
