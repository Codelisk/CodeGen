using Attributes;
using Attributes.ApiAttributes;
using Attributes.WebAttributes.Database;
using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository;
using Attributes.WebAttributes.Repository.Base;
using Foundation.Crawler.Models;
using Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Crawler.Crawlers
{
    public static class AttributeCrawler
    {
        public static INamedTypeSymbol BaseApi(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseApiAttribute)).First();
        }
        public static INamedTypeSymbol BaseContext(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseContextAttribute)).First();
        }
        public static IEnumerable<INamedTypeSymbol> Dtos(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(DtoAttribute));
        }
        public static DefaultRepositoryModel DefaultRepository(this GeneratorExecutionContext context)
        {
            var repo= context.GetClassesWithAttribute(nameof(DefaultRepositoryAttribute)).First();

            return new DefaultRepositoryModel(repo,
                repo.GetMethodsWithAttribute(nameof(GetAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(GetAllAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(SaveAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(DeleteAttribute)).First());
        }
        public static string AttributeUrl(this string attributeValue, INamedTypeSymbol dto, bool plural = false)
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return $"{attributeValue}{dto.ReplaceDtoSuffix(plural)}";
        }
        public static string AttributeUrl<TAttribute>(this GeneratorExecutionContext context, INamedTypeSymbol dto, bool plural = false) where TAttribute : BaseHttpAttribute
        {
            // Find the property with the Url attribute
            var attributeSymobl = context.GetClass<TAttribute>(nameof(Attributes));
            var urlProperty = attributeSymobl.GetMembers().OfType<IPropertySymbol>()
                .FirstOrDefault(property => property.GetAttributes().Any(attr => attr.AttributeClass.Name == typeof(UrlAttribute).Name));
            var test = attributeSymobl.GetMembers().OfType<IPropertySymbol>().Select(x => x).ToList();
            if (urlProperty != null)
            {
                // Resolve the property's default value using the semantic model
                var urlPropertyDeclaration = urlProperty.DeclaringSyntaxReferences.FirstOrDefault();
                var urlDefaultValue = context.Compilation.GetSemanticModel(urlPropertyDeclaration.SyntaxTree).GetConstantValue(urlPropertyDeclaration.GetSyntax());

                if (urlDefaultValue.HasValue)
                {
                    return urlDefaultValue.Value?.ToString().AttributeUrl(dto, plural);
                }
            }

            return null; // If the Url property is not found or it doesn't have a default value.
        }

        public static string AttributeUrl<TAttribute>(this TAttribute attribute, INamedTypeSymbol dto, bool plural = false) where TAttribute : BaseHttpAttribute
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return AttributeUrl(attribute.UrlPrefix, dto, plural);
        }
        public static INamedTypeSymbol BaseRepository(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseRepositoryAttribute)).First();
        }
        public static INamedTypeSymbol BaseController(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(DefaultControllerAttribute)).First();
        }
        public static IPropertySymbol GetIdProperty(this INamedTypeSymbol dto)
        {
            return dto.BaseType.GetPropertyWithAttribute(nameof(IdAttribute));
        }
    }
}
