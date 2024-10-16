using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebGenerator.Base
{
    public static class Extensions
    {
        public static ClassBuilder AddEntityUsing(
            this ClassBuilder classBuilder,
            AttributeCompilationCrawler attributeCompilationCrawler,
            Compilation compilation
        )
        {
            var namespaces = attributeCompilationCrawler
                .Dtos()
                .Select(x => x.EntityFromDto(compilation).GetNamespace())
                .Distinct();
            foreach (var n in namespaces)
            {
                classBuilder.AddNamespaceImport(n);
            }

            return classBuilder;
        }

        public static string DbContextNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}DbContext";
        }

        public static string RepositoryNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Repository";
        }

        public static string RepositoryNameFromDto(this RecordDeclarationSyntax dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Repository";
        }

        public static string ManagerNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Manager";
        }

        public static string ManagerNameFromDto(this RecordDeclarationSyntax dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Manager";
        }

        public static string ControllerNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Controller";
        }

        public static string HttpControllerAttribute(
            this IMethodSymbol method,
            INamedTypeSymbol dto,
            string httpControllerAttribute
        )
        {
            return method.MethodName(dto).AttributeWithConstructor(httpControllerAttribute);
        }

        public static string MethodName(this IMethodSymbol method, INamedTypeSymbol dto)
        {
            var attribute = method.HttpAttribute();
            return attribute.AttributeUrl(dto);
        }

        public static INamedTypeSymbol HttpAttribute(this IMethodSymbol method)
        {
            return method.GetAttributeWithBaseType(typeof(BaseHttpAttribute)).AttributeClass;
        }

        public static IEnumerable<AttributeSyntax> HttpAttribute(
            this MethodDeclarationSyntax method
        )
        {
            return method.GetAttributesDerivedFrom(nameof(BaseHttpAttribute));
        }

        public static INamedTypeSymbol EntityFromDto(this INamedTypeSymbol dto, Compilation context)
        {
            return context
                .GetClassesWithAttribute(nameof(EntityAttribute))
                .FirstOrDefault(x =>
                    (
                        x.GetAttribute<EntityAttribute>()
                            .GetFirstConstructorArgumentAsTypedConstant()
                            .Value as INamedTypeSymbol
                    ).Name == dto.Name
                );
        }

        public static INamedTypeSymbol ConstructFromDto(
            this INamedTypeSymbol symbol,
            INamedTypeSymbol dto,
            Compilation context
        )
        {
            var entity = dto.EntityFromDto(context);
            var idProperty = dto.GetIdProperty();

            if (symbol.TypeArguments.Length == 3)
            {
                return symbol.Construct(dto, idProperty.Type, entity);
            }
            else if (symbol.TypeArguments.Length == 2)
            {
                return symbol.Construct(entity, idProperty.Type);
            }
            else
            {
                return symbol.Construct(entity);
            }
        }

        public static string GetRealManagerName(
            this INamedTypeSymbol defaultManager,
            INamedTypeSymbol dto
        )
        {
            return defaultManager
                .ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                .Replace(defaultManager.Name, dto.ManagerNameFromDto());
        }
    }
}
