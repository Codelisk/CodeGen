using System.Collections.Immutable;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Extensions.New;
using Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions
{
    public static class DtoExtensions
    {
        public static bool DtoHasForeignKeyAttribute(this INamedTypeSymbol dto)
        {
            var result = dto.DtoForeignProperties().Any();
            return result;
        }

        public static bool DtoHasForeignKeyAttribute(
            this RecordDeclarationSyntax dto,
            ImmutableArray<RecordDeclarationSyntax> baseDtos
        )
        {
            var result = dto.DtoForeignProperties(baseDtos).Any();
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

        public static string GetFullTypeName(this RecordDeclarationSyntax c)
        {
            var fullTypeName = c.Identifier.Text;

            // Traverse any containing types (nested types)
            var parent = c.Parent;
            while (parent is TypeDeclarationSyntax typeDeclaration)
            {
                fullTypeName = $"{typeDeclaration.Identifier.Text}.{fullTypeName}";
                parent = parent.Parent;
            }

            // Get the namespace, if available
            var namespaceName = c.GetNamespace();
            if (!string.IsNullOrEmpty(namespaceName))
            {
                fullTypeName = $"{namespaceName}.{fullTypeName}";
            }

            return fullTypeName;
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

        public static string ReplaceDtoSuffix(this RecordDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.GetName().ReplaceLast("Dto", "");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
