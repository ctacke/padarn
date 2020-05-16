namespace System
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;

    public static class TypeExtensions
    {
        private static Dictionary<Type, ConstructorInfo> m_ctorCache = new Dictionary<Type, ConstructorInfo>();

        public static bool Implements<TInterface>(this Type baseType)
        {
            if (!(typeof(TInterface).IsInterface))
            {
                throw new ArgumentException("TInterface must be an interface type.");
            }

            return baseType.GetInterfaces().Contains(typeof(TInterface));
        }

        public static bool Implements(this Type instanceType, Type interfaceType)
        {
            if (!(interfaceType.IsInterface))
            {
                throw new ArgumentException("interfaceType must be an interface type.");
            }

            return instanceType.GetInterfaces().Contains(interfaceType);
        }

        public static ConstructorInfo GetDefaultConstructor(this Type objectType, bool includePrivateConstructor)
        {
            if (m_ctorCache.ContainsKey(objectType))
            {
                return m_ctorCache[objectType];
            }

            var flags = BindingFlags.Public | BindingFlags.Instance;
            if (includePrivateConstructor)
            {
                flags |= BindingFlags.NonPublic;
            }

            var ctor = objectType.GetConstructor(flags, null, new Type[0], null);
            m_ctorCache.Add(objectType, ctor);
            return ctor;
        }

        public static bool IsNullable(this Type objectType)
        {
            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }

            return false;
        }

        public static object[] GetCustomAttributes<T>(this Type type, bool inherit, bool searchInterfaces)
        {
            if (!searchInterfaces)
            {
                return type.GetCustomAttributes(inherit);
            }

            var attributeType = typeof(T);

            return type.GetCustomAttributes(attributeType, inherit)
                .Union(type.GetInterfaces()
                .SelectMany(interfaceType => interfaceType.GetCustomAttributes(attributeType, true)))
                .Distinct()
                .ToArray();
        }
    }
}
