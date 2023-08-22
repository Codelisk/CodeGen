
using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class AttributeDataExtensions
    {
        public static string GetFirstConstructorArgument(this AttributeData attributeData)
        {
            if (attributeData.ConstructorArguments.Any())
            {
                return attributeData.ConstructorArguments.FirstOrDefault().Value + string.Empty;
            }
            return null;
        }
        public static TypedConstant GetFirstConstructorArgumentAsTypedConstant(this AttributeData attributeData)
        {
            return attributeData.ConstructorArguments.FirstOrDefault();
        }
        public static T GetFirstConstructorArgumentEnum<T>(this AttributeData attributeData) where T : Enum
        {
            return (T)attributeData.ConstructorArguments.FirstOrDefault().Value;
        }
        public static TCustomAttr GetRealAttributeFromAttribute<TCustomAttr>(this Type t)
        {
            var result = t.GetCustomAttributes(true)
                                      .OfType<TCustomAttr>()
                                      .FirstOrDefault();

            return result;
        }
        public static string GetAttributeName(this AttributeData attributeData)
        {
            return attributeData.AttributeClass.Name;
        }
        public static AttributeData GetAttributeWithName(this IEnumerable<AttributeData> attributeData, string name)
        {
            return attributeData.FirstOrDefault(x => GetAttributeName(x).Equals(name));
        }
        public static string GetAttributePropertyDefaultValue<T>(AttributeData attributeData) where T : Attribute
        {
            throw new NotImplementedException();
            var property = attributeData.AttributeClass.GetPropertyWithAttribute(nameof(T)).GetAttributes().First();

            return property.NamedArguments.First().Value.Value.ToString();
        }
    }
}
