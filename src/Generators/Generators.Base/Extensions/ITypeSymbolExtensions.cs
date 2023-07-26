using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class ITypeSymbolExtensions
    {
        
        // Check if a given type is IEnumerable<> (or inherits from it)
        public static bool IsEnumerableType(this ITypeSymbol typeSymbol, Compilation compilation)
        {
            // Get the IEnumerable<> interface
            var enumerableInterface = compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");

            // Check if the type implements the IEnumerable<> interface or inherits from it
            return typeSymbol.AllInterfaces.Any(interfaceType =>
                interfaceType.OriginalDefinition.Equals(enumerableInterface));
        }
    }
}
