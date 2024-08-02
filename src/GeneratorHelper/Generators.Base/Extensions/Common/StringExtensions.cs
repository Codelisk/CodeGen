namespace Generators.Base.Extensions.Common
{
    public static class StringExtensions
    {
        public static string ReplaceLast(this string source, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(oldValue))
            {
                return source;
            }

            int lastIndex = source.LastIndexOf(oldValue);
            if (lastIndex == -1)
            {
                return source;
            }

            string result = source.Remove(lastIndex, oldValue.Length).Insert(lastIndex, newValue);
            return result;
        }

        public static string RealAttributeName(this string attributeName)
        {
            return attributeName.Replace("Attribute", "");
        }

        public static string RealAttributeName(this Type attributeType)
        {
            return attributeType.Name.RealAttributeName();
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
