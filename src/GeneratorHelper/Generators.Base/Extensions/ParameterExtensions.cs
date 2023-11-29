using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class ParameterExtensions
    {
        public static string GetParameterName(this string typeName, bool plural=false)
        {
            var result = char.ToLower(typeName[0]) + typeName.Substring(1);
            if (plural)
            {
                result += "List";
            }

            return result;
        }
    }
}
