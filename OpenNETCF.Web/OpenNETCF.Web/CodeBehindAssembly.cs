using System.IO;

namespace OpenNETCF.Web
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Helper class for loading a code-behind assembly into the Web Server
    /// </summary>
    public sealed class CodeBehindAssembly
    {
        private Assembly asm;

        private CodeBehindAssembly(Assembly asm)
        {
            this.asm = asm;
        }

        /// <summary>
        /// Loads a code-behind assembly from the specified path.
        /// </summary>
        /// <param name="assemblyPath">The path to load the assembly from.</param>
        /// <returns>An instance of CodeBehindAssembly</returns>
        public static CodeBehindAssembly LoadFrom(string assemblyPath)
        {
            try
            {
                return new CodeBehindAssembly(Assembly.LoadFrom(assemblyPath));
            }
            catch (IOException ioe)
            {
                if (ioe.Message.EndsWith("was not found."))
                {
                    throw new FileNotFoundException(ioe.Message);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an array of Types with a particular base type.
        /// </summary>
        /// <param name="baseType">The base type used to filter the array</param>
        /// <returns>An array of types of type baseType</returns>
        public Type[] GetTypesFromBaseType(Type baseType)
        {
            Type[] types = null;
            try
            {
                types = asm.GetTypes();
            }
            catch(TypeLoadException)
            {
                return null;
            }

            if (types == null)
            {
                return types;
            }

            int count = types.Length;

            List<Type> matchedTypes = new List<Type>();

            for (int i = 0; i < count; i++)
            {
                if (types[i] != null)
                {
                    if (types[i].IsSubclassOf(baseType))
                    {
                        matchedTypes.Add(types[i]);
                    }
                }
            }

            return matchedTypes.ToArray();
        }
    }
}
