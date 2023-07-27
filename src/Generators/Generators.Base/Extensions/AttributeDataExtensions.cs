using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class AttributeDataExtensions
    {
        public static string GetFirstConstructorArgument(this AttributeData attributeData)
        {
            if(attributeData.ConstructorArguments.Any())
            {
                return attributeData.ConstructorArguments.FirstOrDefault().Value + string.Empty;
            }
            return null;
        }
        public static string GetAttributeName(this AttributeData attributeData)
        {
            return attributeData.AttributeClass.Name;
        }
        public static AttributeData GetAttributeWithName(this IEnumerable<AttributeData> attributeData, string name)
        {
            return attributeData.FirstOrDefault(x=>GetAttributeName(x).Equals(name));
        }
        public static string GetAttributePropertyDefaultValue<T>(AttributeData attributeData) where T : Attribute
        {
            throw new NotImplementedException();
            var property = attributeData.AttributeClass.GetPropertyWithAttribute(nameof(T)).GetAttributes().First();

            return property.NamedArguments.First().Value.Value.ToString();
        }
    }
}
