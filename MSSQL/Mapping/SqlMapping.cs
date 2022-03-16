using MSSQL.Reflection;
using System;
using System.Reflection;

namespace MSSQL.Mapping
{
    internal class SqlMapping
    {
        private ObjReflection objReflection;

        public SqlMapping()
        {
            objReflection = new ObjReflection();
        }

        public string GetTableName<T>(bool enclosedInSquareBrackets = false)
        {
            string objectName = objReflection.GetObjectName<T>();
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetTableName(object obj, bool enclosedInSquareBrackets = false)
        {
            string objectName = objReflection.GetObjectName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetTableName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            string objectName = GetTableName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetPropertyName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            return (enclosedInSquareBrackets) ? "[" + propertyInfo.Name + "]" : propertyInfo.Name;
        }
    }
}
