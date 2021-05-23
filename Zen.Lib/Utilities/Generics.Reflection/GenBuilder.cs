using System;
using System.Collections.Generic;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// Generics Builder
    /// </summary>
    public sealed class GenBuilder
    {
        static readonly Type KVType = typeof(KeyValuePair<,>);

        public static Type BuildType(Type genericT, Type[] typeArguments)
        {
            return genericT.MakeGenericType(typeArguments);
        }
        public static Type BuildKeyValueType(Type dictionaryT)
        {
            if (!TypeInterrogator.IsDictionaryType(dictionaryT))
            {
                System.Diagnostics.Debug.Assert(false);
                return null;
            }

            return KVType.MakeGenericType(TypeInterrogator.GetItemTypes(dictionaryT));
        }
        public static object BuildInstance(Type genericT, Type[] typeArguments)
        {
            Type genericType = genericT.MakeGenericType(typeArguments);
            return Activator.CreateInstance(genericType);
        }
        public static object BuildInstance(Type genericT, Type[] typeArguments, object[] args)
        {
            Type genericType = genericT.MakeGenericType(typeArguments);
            return Activator.CreateInstance(genericType, args);
        }
        public static object BuildInstance(Type objType, object[] args)
        {
            return Activator.CreateInstance(objType, args);
        }
    }
}
