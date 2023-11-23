using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Shared.Constants;

namespace Generators.Base.Extensions
{
    public static class IPropertySymbolExtensions
    {
        public static string GetPropertyAttributeValue(this IPropertySymbol propertySymbol, string attributeName)
        {
            return propertySymbol.GetAllAttributes().First(x => x.AttributeClass.Name.Equals(attributeName)).ConstructorArguments.First().Value.ToString();
        }
    }
}
