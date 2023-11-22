using Generators.Base;
using Microsoft.CodeAnalysis;
using Generators.Base.Extensions;

namespace Foundation.Crawler.Extensions
{
    public static class DtoExtensions
    {
        public static string GetFullModelNameFromProperty(this IPropertySymbol foreignKeyProperty)
        {
            return foreignKeyProperty.Name.GetParameterName().Replace("Id","").Replace("id","");
        }
        public static string GetFullModelName(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.Replace("Dto", "Full");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
        public static string ReplaceDtoSuffix(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.Replace("Dto", "");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
