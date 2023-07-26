using CodeGenHelpers.Internals;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class ISymbolExtensions
    {
        public static bool HasAttribute(this ISymbol symbol, string fullAttributeName) => symbol.GetAllAttributes().Any(x => x?.AttributeClass is not null &&
                                                                                                                x.AttributeClass.Name ==
                                                                                                                fullAttributeName);

        public static IEnumerable<AttributeData?> GetAllAttributes(this ISymbol? symbol)
        {
            while (symbol != null && symbol.Name != typeof(object).Name)
            {
                var attributes = symbol.GetAttributes();
                foreach (var attribute in attributes)
                {
                    yield return attribute;
                }

                if (symbol is INamedTypeSymbol namedTypeSymbol)
                {
                    symbol = namedTypeSymbol.BaseType;
                }
                else
                {
                    symbol = null;
                }
            }
        }
    }
}
