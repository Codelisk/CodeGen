using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;
using Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New
{
    public static class DtoExtensions
    {
        public static IEnumerable<PropertyDeclarationSyntax> DtoForeignProperties(
            this RecordDeclarationSyntax dto,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            return dto.DtoProperties(baseDtos)
                .Where(x => x.HasAttribute(AttributeNames.ForeignKey));
        }

        public static IncrementalValueProvider<ImmutableArray<RecordDeclarationSyntax>> Dtos(
            this IncrementalGeneratorInitializationContext context
        )
        {
            var dtos = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DtoAttribute).FullName,
                    static (n, _) => n is RecordDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        RecordDeclarationSyntax classDeclarationSyntax = (RecordDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<DtoAttribute>()
                            ? classDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            var tenantDtos = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(TenantDtoAttribute).FullName,
                    static (n, _) => n is RecordDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        RecordDeclarationSyntax classDeclarationSyntax = (RecordDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<TenantDtoAttribute>()
                            ? classDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            var combinedDtosProvider = dtos.Combine(tenantDtos)
                .Select(
                    (combined, _) =>
                    {
                        var (dtos, tenantDtos) = combined;
                        return dtos.Concat(tenantDtos).Distinct().ToImmutableArray();
                    }
                );
            return combinedDtosProvider!;
        }

        public static IncrementalValueProvider<ImmutableArray<RecordDeclarationSyntax>> BaseDtos(
            this IncrementalGeneratorInitializationContext context
        )
        {
            var dtos = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DtoBaseAttribute).FullName,
                    static (n, _) => n is RecordDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        RecordDeclarationSyntax classDeclarationSyntax = (RecordDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<DtoBaseAttribute>()
                            ? classDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            return dtos;
        }

        public static IEnumerable<PropertyDeclarationSyntax> DtoProperties(
            this RecordDeclarationSyntax dto,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            // Annahme: dto ist eine RecordDeclarationSyntax oder ClassDeclarationSyntax
            var allProperties = new List<PropertyDeclarationSyntax>();

            // Füge die Properties der aktuellen Klasse (dto) hinzu
            allProperties.AddRange(dto.GetProperties());

            // Extrahiere rekursiv die Properties der Basisklassen
            ExtractBaseProperties(dto.BaseList, baseDtos, allProperties);
            return allProperties;
        }

        public static string GetFullModelNameFromProperty(
            this PropertyDeclarationSyntax foreignKeyProperty
        )
        {
            var result = foreignKeyProperty
                .GetPropertyName()
                .GetParameterName()
                .ReplaceLast("Id", "")
                .ReplaceLast("id", "");

            return result;
        }

        public static string GetFullModelName(this RecordDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.ReplaceLast("Dto", "Full");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        public static string GetEntityName(this RecordDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.ReplaceLast("Dto", "Entity");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        static void ExtractBaseProperties(
            BaseListSyntax baseList,
            IEnumerable<RecordDeclarationSyntax> baseDtos,
            List<PropertyDeclarationSyntax> properties
        )
        {
            if (baseList == null)
                return; // Keine Basisklasse vorhanden

            foreach (var baseTypeSyntax in baseList.Types)
            {
                // Hole den Namen der Basisklasse
                var baseTypeName = baseTypeSyntax.Type.GetName();

                if (!string.IsNullOrEmpty(baseTypeName))
                {
                    // Suche nach der Basisklasse in den bekannten baseDtos
                    var baseDto = baseDtos.FirstOrDefault(x => x.Identifier.Text == baseTypeName);

                    if (baseDto != null)
                    {
                        // Füge die Properties der Basisklasse hinzu
                        properties.AddRange(baseDto.GetProperties());

                        // Rekursiv weitermachen, falls diese Basisklasse ebenfalls eine Basisklasse hat
                        ExtractBaseProperties(baseDto.BaseList, baseDtos, properties);
                    }
                }
            }
        }
    }
}
