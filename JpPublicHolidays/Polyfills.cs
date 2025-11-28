// This file provides polyfills for features not available in .NET Standard 2.0/2.1

#if NETSTANDARD2_0 || NETSTANDARD2_1
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Polyfill for init accessor support in .NET Standard.
    /// </summary>
    internal static class IsExternalInit { }
}
#endif
