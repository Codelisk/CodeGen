namespace Generators.Base.Extensions.Common
{
    public static class StringExtensions
    {
        public static string RealAttributeName(this Type attributeType)
        {
            return attributeType.Name.Replace("Attribute", "");
        }

        public static string AttributeWithConstructor(
            this string constructorValue,
            string attributeName
        )
        {
            return $"{attributeName}(\"{constructorValue}\")";
        }
    }
}
