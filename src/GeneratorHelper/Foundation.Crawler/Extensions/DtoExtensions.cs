using Generators.Base;
using Microsoft.CodeAnalysis;
using Generators.Base.Extensions;
using Shared.Constants;

namespace Foundation.Crawler.Extensions
{
    public static class DtoExtensions
    {
        public static bool DtoHasForeignKeyAttribute(this INamedTypeSymbol dto)
        {
            var result = dto.DtoForeignProperties().Any();
            return result;
        }
        public static IEnumerable<IPropertySymbol> DtoForeignProperties(this INamedTypeSymbol dto)
        {
            var result = dto.GetAllProperties(true).Where(x => x.GetAllAttributes().Any(x => x.AttributeClass.Name.Equals(AttributeNames.ForeignKey)));
            return result;
        }
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
