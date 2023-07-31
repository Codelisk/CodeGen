using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class EnumerableINamedTypeSymbolExtensions
    {
        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttribute(this IEnumerable<INamedTypeSymbol> classSymbols, string fullAttributeName, string assemblyName = "")
        {
            return classSymbols.Where(x => x.HasAttributeWithoutBaseClass(fullAttributeName));
        }
    }
}
