using Codelisk.GeneratorShared.Constants;
using Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;

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
            var result = dto.GetAllProperties(true)
                .Where(x =>
                    x.GetAllAttributes()
                        .Any(x => x.AttributeClass.Name.Equals(AttributeNames.ForeignKey))
                );
            return result;
        }

        public static string GetFullModelNameFromProperty(this IPropertySymbol foreignKeyProperty)
        {
            return foreignKeyProperty
                .Name.GetParameterName()
                .ReplaceLast("Id", "")
                .ReplaceLast("id", "");
        }

        public static string GetFullTypeName(this INamedTypeSymbol dto)
        {
            return dto.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        public static string GetFullModelName(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.ReplaceLast("Dto", "Full");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        public static string GetEntityName(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.ReplaceLast("Dto", "Entity");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        public static string ReplaceDtoSuffix(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.ReplaceLast("Dto", "");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
