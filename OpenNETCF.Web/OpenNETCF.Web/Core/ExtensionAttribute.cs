#if NET_20
namespace System.Runtime.CompilerServices
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}
#endif