namespace Generators.Base.Extensions
{
    public static class ParameterExtensions
    {
        public static string GetParameterName(this string typeName)
        {
            return char.ToLower(typeName[0]) + typeName.Substring(1);
        }
    }
}
