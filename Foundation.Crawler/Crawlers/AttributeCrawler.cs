using Attributes;
using Attributes.ApiAttributes;
using Attributes.WebAttributes.Database;
using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository;
using Attributes.WebAttributes.Repository.Base;
using Foundation.Crawler.Extensions;
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
        public static INamedTypeSymbol GetAttribute<TAttribute>(this GeneratorExecutionContext context) where TAttribute : Attribute
        {
            // Find the property with the Url attribute
            return context.GetClass<TAttribute>(nameof(Attributes));
        }
        public static string AttributeUrl(this GeneratorExecutionContext context, Type t, INamedTypeSymbol dto)
        {
            // Find the property with the Url attribute
            var attributeSymobl = context.GetClass(t, nameof(Attributes));
            return attributeSymobl.AttributeUrl(dto);
        }
        public static string AttributeUrl<TAttribute>(this GeneratorExecutionContext context, INamedTypeSymbol dto) where TAttribute : BaseHttpAttribute
        {
            // Find the property with the Url attribute
            var attributeSymobl = context.GetAttribute<TAttribute>();
            return attributeSymobl.AttributeUrl(dto);
        }
        public static string AttributeUrl(this INamedTypeSymbol attributeSymobl, INamedTypeSymbol dto)
        {
            var urlProperty = attributeSymobl.GetAttribute<UrlAttribute>();
            bool plural = attributeSymobl.HasAttribute(nameof(PluralAttribute));
            return urlProperty.GetFirstConstructorArgument().AttributeUrl(dto, plural);
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
