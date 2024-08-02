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
            this ClassDeclarationSyntax dto
        )
        {
            var result = dto.GetAllProperties(true, false)
                .Where(x => x.HasAttribute(AttributeNames.ForeignKey));
            return result;
        }

        public static IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax>> Dtos(
            this IncrementalGeneratorInitializationContext context
        )
        {
            var dtos = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DtoAttribute).FullName,
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
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
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
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

        private static string GetNamespace(SyntaxNode node)
        {
            var nameSpace = string.Empty;
            for (SyntaxNode? current = node; current != null; current = current.Parent)
            {
                if (current is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    nameSpace = namespaceDeclaration.Name.ToString() + "." + nameSpace;
                }
                else if (current is FileScopedNamespaceDeclarationSyntax fileScopedNamespace)
                {
                    nameSpace = fileScopedNamespace.Name.ToString() + "." + nameSpace;
                }
                else if (current is CompilationUnitSyntax)
                {
                    // Reached the root, stop traversing
                    break;
                }
            }
            return nameSpace.TrimEnd('.');
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

        public static string GetFullModelName(this ClassDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.ReplaceLast("Dto", "Full");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        public static string GetEntityName(this ClassDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.ReplaceLast("Dto", "Entity");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
