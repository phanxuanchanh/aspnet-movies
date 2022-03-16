using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MSSQL.Reflection
{
    using CustomAttribute = CustomAttributeData;
    using NamedArgument = CustomAttributeNamedArgument;
    internal class ObjReflection
    {
        public ObjReflection()
        {

        }

        public T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        public string GetObjectName(object obj)
        {
            return obj.GetType().Name;
        }

        public string GetObjectName<T>()
        {
            return typeof(T).Name;
        }

        public CustomAttribute[] GetCustomAttributes(object obj)
        {
            return obj.GetType().CustomAttributes.ToArray();
        }

        public CustomAttribute[] GetCustomAttributes<T>()
        {
            return typeof(T).CustomAttributes.ToArray();
        }

        public CustomAttribute GetCustomAttribute(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes(obj).SingleOrDefault(predicate);
        }

        public CustomAttribute[] GetCustomAttributes(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes(obj).Where(predicate).ToArray();
        }

        public CustomAttribute GetCustomAttribute<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes<T>().SingleOrDefault(predicate);
        }

        public CustomAttribute[] GetCustomAttributes<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes<T>().Where(predicate).ToArray();
        }

        public CustomAttribute[] GetCustomAttributes
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must not be null");
                return null;
            }
            return propertyInfo.CustomAttributes.Where(predicate).ToArray();
        }

        public CustomAttribute[] GetCustomAttributes(PropertyInfo propertyInfo, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must not be null");
                return null;
            }
            return propertyInfo.CustomAttributes.ToArray();
        }

        public CustomAttribute GetCustomAttribute
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must not be null");
                return null;
            }
            return propertyInfo.CustomAttributes.SingleOrDefault(predicate);
        }

        public NamedArgument[] GetNamedArguments
            (object obj, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must not be null");
                return null;
            }
            return customAttributeData.NamedArguments.ToArray();
        }

        public NamedArgument[] GetNamedArguments
            (object obj, Func<CustomAttribute, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must not be null");
                return null;
            }
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public NamedArgument[] GetNamedArguments<T>
            (Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must not be null");
                return null;
            }
            return customAttributeData.NamedArguments.ToArray();
        }

        public NamedArgument[] GetNamedArguments<T>
            (Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must not be null");
                return null;
            }
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public NamedArgument GetNamedArgument
            (object obj, Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must not be null");
                return default(NamedArgument);
            }
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public NamedArgument GetNamedArgument<T>
            (Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public NamedArgument[] GetNamedArguments
            (CustomAttribute customAttributeData)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public NamedArgument[] GetNamedArguments
            (CustomAttribute customAttributeData, Func<NamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate).ToArray();
        }

        public NamedArgument GetNamedArgument
            (CustomAttribute customAttributeData, Func<NamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate);
        }

        public NamedArgument[] GetNamedArguments
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(propertyInfo, predicate);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public NamedArgument[] GetNamedArguments
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(propertyInfo, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public NamedArgument GetNamedArgument
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttribute = GetCustomAttribute(propertyInfo, predicate1);
            if (customAttribute == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("");
                return default(NamedArgument);
            }
            return customAttribute.NamedArguments.SingleOrDefault(predicate2);
        }

        public PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        public PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties();
        }

        public PropertyInfo[] GetProperties(object obj, Func<PropertyInfo, bool> predicate)
        {
            return GetProperties(obj).Where(predicate).ToArray();
        }

        public PropertyInfo[] GetProperties<T>(Func<PropertyInfo, bool> predicate)
        {
            return GetProperties<T>().Where(predicate).ToArray();
        }

        public PropertyInfo GetProperty(object obj, Func<PropertyInfo, bool> predicate)
        {
            return GetProperties(obj).SingleOrDefault(predicate);
        }

        public PropertyInfo GetProperty<T>(Func<PropertyInfo, bool> predicate)
        {
            return GetProperties<T>().SingleOrDefault(predicate);
        }

        public PropertyInfo[] GetProperties(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetProperties(obj, p => p.CustomAttributes.Any(predicate));
        }

        public PropertyInfo[] GetProperties<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetProperties<T>(p => p.CustomAttributes.Any(predicate));
        }

        public PropertyInfo GetProperty(object obj, string propertyName)
        {
            return GetProperty(obj, p => p.Name == propertyName);
        }

        public PropertyInfo GetProperty<T>(string propertyName)
        {
            return GetProperty<T>(p => p.Name == propertyName);
        }

        public PropertyInfo[] GetProperties(object obj, string customAttributeName)
        {
            return GetProperties(obj, c => c.AttributeType.Name == customAttributeName);
        }

        public PropertyInfo[] GetProperties<T>(string customAttributeName)
        {
            return GetProperties<T>(c => c.AttributeType.Name == customAttributeName);
        }

        public CustomAttributeData GetCustomAttribute(object obj, string customAttributeName)
        {
            return GetCustomAttribute(obj, c => c.AttributeType.Name == customAttributeName);
        }

        public CustomAttribute GetCustomAttribute<T>(string customAttributeName)
        {
            return GetCustomAttribute<T>(c => c.AttributeType.Name == customAttributeName);
        }

        public NamedArgument GetNamedArgument(PropertyInfo propertyInfo, string memberName)
        {
            CustomAttributeData customAttributeData = propertyInfo.CustomAttributes
                    .SingleOrDefault(c => c.AttributeType.Name == "Column" || c.AttributeType.Name == "PrimaryKey");
            if (customAttributeData != null)
                throw new Exception("");

            return customAttributeData.NamedArguments.SingleOrDefault(n => n.MemberName == memberName);
        }

        public object SetValuesForPropertiesOfObject(object model, Dictionary<string, object> pairs)
        {
            Type type = model.GetType();
            if (type.IsValueType || type.Name == "String")
                return null;
            Dictionary<string, object> properties = pairs.Where(p => Regex.IsMatch(p.Key, "(^.)[a-zA-Z0-9]{1,}$"))
                .ToDictionary(p => p.Key, p => p.Value);
            foreach (KeyValuePair<string, object> property in properties)
            {
                PropertyInfo propertyInfo = GetProperty(model, property.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(model, property.Value);
            }
            return model;
        }
    }
}
