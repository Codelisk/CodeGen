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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New
{
    public static class DtoExtensions
    {
        public static string GetFullTypeName(this TypeDeclarationSyntax c)
        {
            // Start with the class name (Identifier)
            var fullTypeName = c.Identifier.Text;

            // Check if the class has type parameters (generics)
            if (c.TypeParameterList != null && c.TypeParameterList.Parameters.Any())
            {
                var typeParameters = string.Join(
                    ", ",
                    c.TypeParameterList.Parameters.Select(p => p.Identifier.Text)
                );
                fullTypeName += $"<{typeParameters}>";
            }

            // Traverse any containing types (to handle nested types)
            var parent = c.Parent;
            while (parent is TypeDeclarationSyntax typeDeclaration)
            {
                var parentTypeName = typeDeclaration.Identifier.Text;

                // Handle generics for the containing type
                if (
                    typeDeclaration is ClassDeclarationSyntax parentClass
                    && parentClass.TypeParameterList != null
                    && parentClass.TypeParameterList.Parameters.Any()
                )
                {
                    var parentTypeParameters = string.Join(
                        ", ",
                        parentClass.TypeParameterList.Parameters.Select(p => p.Identifier.Text)
                    );
                    parentTypeName += $"<{parentTypeParameters}>";
                }

                fullTypeName = $"{parentTypeName}.{fullTypeName}";
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

        public static ClassDeclarationSyntax Construct(
            this ClassDeclarationSyntax classDeclaration,
            RecordDeclarationSyntax dto
        )
        {
            // Check if the dto has a TypeParameterList
            if (classDeclaration.TypeParameterList == null)
            {
                // If no type parameters, return the original class declaration
                return classDeclaration;
            }

            // Create a new ClassDeclarationSyntax with the same name and modifiers
            var typeParameters = classDeclaration
                .TypeParameterList.Parameters.Select(tp =>
                    SyntaxFactory.TypeParameter(ReplaceConstructValue(dto, tp.Identifier.Text))
                )
                .ToArray();
            // Create a new TypeParameterListSyntax
            var newTypeParameterList = SyntaxFactory.TypeParameterList(
                SyntaxFactory.SeparatedList(typeParameters)
            );
            // Create the new ClassDeclarationSyntax with the new type parameters
            return classDeclaration.WithTypeParameterList(newTypeParameterList);
        }

        public static string ReplaceConstructValue(this RecordDeclarationSyntax dto, string name)
        {
            name = name.Replace("TEntity", dto.GetEntityName());
            name = name.Replace("TDto", dto.GetName());
            name = name.Replace("TKey", "Guid");
            return name;
        }

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

        public static IncrementalValueProvider<
            ImmutableArray<InterfaceDeclarationSyntax>
        > BaseDtoInterfaces(this IncrementalGeneratorInitializationContext context)
        {
            var dtos = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DtoBaseInterfaceAttribute).FullName,
                    static (n, _) => n is InterfaceDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        InterfaceDeclarationSyntax interfaceDeclarationSyntax =
                            (InterfaceDeclarationSyntax)context.TargetNode;

                        return interfaceDeclarationSyntax.HasAttribute<DtoBaseInterfaceAttribute>()
                            ? interfaceDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            return dtos;
        }

        public static IEnumerable<BaseTypeSyntax> DtoInterfaces(
            this RecordDeclarationSyntax dto,
            ImmutableArray<RecordDeclarationSyntax> baseDtos,
            ImmutableArray<InterfaceDeclarationSyntax> baseInterfaces
        )
        {
            var allInterfaces = new HashSet<BaseTypeSyntax>();
            FindInterfacesRecursively(dto, baseDtos, baseInterfaces, allInterfaces);
            return allInterfaces;
        }

        private static void FindInterfacesRecursively(
            RecordDeclarationSyntax dto,
            ImmutableArray<RecordDeclarationSyntax> baseDtos,
            ImmutableArray<InterfaceDeclarationSyntax> baseInterfaces,
            HashSet<BaseTypeSyntax> allInterfaces
        )
        {
            // Überprüfe, ob das aktuelle DTO Interfaces hat
            if (dto.BaseList != null)
            {
                foreach (var baseType in dto.BaseList.Types)
                {
                    // Überprüfe auch, ob das BaseType selbst ein Interface ist
                    if (baseInterfaces.Any(i => i.Identifier.Text == baseType.Type.ToString()))
                    {
                        allInterfaces.Add(baseType);
                    }

                    var typeName = baseType.Type.GetName();
                    // Finde die Basis-DTO
                    var baseDto = baseDtos.FirstOrDefault(b =>
                        b.Identifier.Text == baseType.Type.GetName()
                    );

                    // Wenn die Basis-DTO gefunden wurde, die Interfaces hinzufügen
                    if (baseDto != null)
                    {
                        // Füge alle Interfaces der Basisklasse hinzu
                        AddInterfaces(baseDto.BaseList, baseInterfaces, allInterfaces);

                        // Rekursiv die Interfaces von den Basisklassen der Basisklasse hinzufügen
                        FindInterfacesRecursively(baseDto, baseDtos, baseInterfaces, allInterfaces);
                    }
                }
            }
        }

        private static void AddInterfaces(
            BaseListSyntax baseList,
            ImmutableArray<InterfaceDeclarationSyntax> baseInterfaces,
            HashSet<BaseTypeSyntax> allInterfaces
        )
        {
            int count = 0;
            foreach (var baseType in baseList.Types)
            {
                if (count > 0)
                {
                    allInterfaces.Add(baseType);
                }
                if (baseInterfaces.Any(i => i.Identifier.Text == baseType.Type.GetName()))
                {
                    allInterfaces.Add(baseType);
                }
                count++;
            }
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
