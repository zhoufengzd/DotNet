using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zen.Utilities
{
    using TypeCategories = Zen.Utilities.Defs.TypeCategories;

    /// <summary>
    /// Use reflection to get property values.
    /// </summary>
    public sealed class PropertyInterrogator
    {
        public static List<string> GetPropertyNames(string typeName)
        {
            Type typeQueried = Type.GetType(typeName);
            return GetPropertyNames(typeQueried);
        }

        public static List<string> GetPropertyNames(Type typeQueried)
        {
            PropertyInfo[] properties = typeQueried.GetProperties();
            List<string> propertyNameList = new List<string>(properties.Length);

            foreach (PropertyInfo property in properties)
            {
                propertyNameList.Add(property.Name);
            }

            return propertyNameList;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            object value = null;
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != null)
                value = property.GetValue(obj, null);

            return value;
        }

        public static string GetFullName(PropertyInfo pi)
        {
            return pi.DeclaringType.FullName + "." + pi.Name;
        }
    }

    public sealed class AttributeInterrogator
    {
        public static string[] GetAttributeNames(Type tp)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(tp);

            string[] attributeNames = new string[attrs.Length];
            int ind = 0;
            foreach (Attribute attr in attrs)
            {
                attributeNames[ind++] = attr.GetType().Name;
            }

            return attributeNames;
        }

        public static T GetAttribute<T>(PropertyInfo pi) where T : Attribute
        {
            object[] attrs = pi.GetCustomAttributes(typeof(T), true);
            if (attrs != null && attrs.Length > 0)
                return (T)attrs[0];
            else
                return null;
        }
    }

    public sealed class TypeInterrogator
    {
        /// <summary>
        /// Reference type: class, delegate (specialized class), array, or interface
        /// </summary>
        public static bool IsReferenceType(Type tp)
        {
            return tp.IsClass || tp.IsInterface || tp.IsArray;
        }
        public static bool IsDelegateType(Type tp)
        {
            return IsSubclass(typeof(Delegate), tp);
        }
        public static bool IsEventType(Type tp)
        {
            //return IsSubclass(typeof(Event), tp);
            return false;
        }

        /// <summary>
        /// Value type: struct (user struct), simple type (numeric-type + bool), enum
        /// </summary>
        public static bool IsValueType(Type tp)
        {
            return IsSubclass(typeof(System.ValueType), tp);
        }
        public static bool IsEnumType(Type tp)
        {
            return (tp.BaseType == typeof(System.Enum));
        }

        /// <summary>
        /// Simple-type: numeric-type + bool
        /// </summary>
        public static bool IsSimpleType(Type tp)
        {
            return IsTypeOf(tp, TypeCategories.SimpleTypes);
        }
        public static bool IsNumericType(Type tp)
        {
            return IsTypeOf(tp, TypeCategories.NumericTypes);
        }
        public static bool IsBoolType(Type tp)
        {
            return tp == typeof(Boolean);
        }
        public static bool IsStrcutType(Type tp) // user struct type
        {
            return !IsEnumType(tp) && !IsSimpleType(tp) && IsValueType(tp);
        }
        public static bool IsStringType(Type tp)
        {
            return tp == typeof(String);
        }
        public static bool IsDateTimeType(Type tp)
        {
            return tp == typeof(DateTime);
        }

        /// <summary>
        /// Single value type =
        ///    simple-type (numeric-type + bool) + enum + string + DateTime
        /// </summary>
        public static bool IsSingleValueType(Type tp)
        {
            return (IsEnumType(tp) || IsStringType(tp) || IsSimpleType(tp) || IsDateTimeType(tp));
        }
        /// <summary>
        /// Collection type = Array type + IEnumerable type
        /// </summary>
        public static bool IsCollectionType(Type tp)
        {
            return (tp.IsArray || HasInterface(typeof(System.Collections.IEnumerable), tp));
        }
        public static bool IsArrayType(Type tp)
        {
            return tp.IsArray;
        }
        /// <summary>
        /// Not Array and has IList interface 
        /// </summary>
        public static bool IsListType(Type tp)
        {
            return (!tp.IsArray) && HasInterface(typeof(System.Collections.IList), tp);
        }
        public static bool IsDictionaryType(Type tp)
        {
            return HasInterface(typeof(System.Collections.IDictionary), tp);
        }

        #region help functions
        public static bool IsSubclass(Type parentType, Type childInQuestion)
        {
            while (childInQuestion != null)
            {
                if (parentType == GetUnderlyingType(childInQuestion))
                    return true;

                childInQuestion = childInQuestion.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Return true if questioned type has the interface or is the interface itself
        /// </summary>
        public static bool HasInterface(Type interfaceType, Type questionedType)
        {
            if (questionedType == interfaceType)
                return true;

            foreach (Type itf in questionedType.GetInterfaces())
            {
                if (itf == interfaceType)
                    return true;
            }

            return false;
        }
        public static Type GetUnderlyingType(Type tp)
        {
            Type underlyingType = tp;

            if (tp.IsGenericType)
            {
                underlyingType = tp.GetGenericTypeDefinition();

                if (underlyingType == typeof(Nullable<>))
                    underlyingType = Nullable.GetUnderlyingType(tp);
            }

            return underlyingType;
        }

        /// <summary>
        /// Get item type for single item generic collections
        /// </summary>
        public static Type GetItemType(Type collectionT)
        {
            if (collectionT.IsArray)
                return collectionT.GetElementType();

            Type[] genericTypes = collectionT.GetGenericArguments();
            return (genericTypes.Length > 0) ? genericTypes[0] : null;
        }

        /// <summary>
        /// Get multiple item type(s) for generic collections, for example,
        ///   Dictionary<KeyT, ValueT> ==> new Type[]{KeyT, ValueT}
        ///   IList<ValueT> ==> new Type[]{ValueT}
        /// </summary>
        public static Type[] GetItemTypes(Type collectionT)
        {
            if (collectionT.IsArray)
                return new Type[]{collectionT.GetElementType()};

            return collectionT.GetGenericArguments();
        }
        #endregion

        #region internal private functions
        private static bool IsTypeOf(Type typeQueried, Type[] tps)
        {
            foreach (Type tp in tps)
            {
                if (typeQueried == tp)
                    return true;
            }

            return false;
        }
        #endregion
    }
}
