using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.ApiAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Database;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

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
        public static IEnumerable<INamedTypeSymbol> Entities(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(EntityAttribute));
        }
        public static INamedTypeSymbol DefaultApiRepository(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(DefaultApiRepositoryAttribute)).First();
        }
        public static string AttributeUrl(this string attributeValue, INamedTypeSymbol dto)
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return $"{attributeValue}";
        }
        public static INamedTypeSymbol GetAttribute<TAttribute>(this GeneratorExecutionContext context) where TAttribute : Attribute
        {
            // Find the property with the Url attribute
            return context.GetClass<TAttribute>("Codelisk.GeneratorAttributes");
        }
        public static string AttributeUrl(this GeneratorExecutionContext context, Type t, INamedTypeSymbol dto)
        {
            // Find the property with the Url attribute
            var attributeSymobl = context.GetClass(t, "Codelisk.GeneratorAttributes");
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
            return urlProperty.GetFirstConstructorArgument().AttributeUrl(dto);
        }
        public static INamedTypeSymbol Manager(this GeneratorExecutionContext context, INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultManagerAttribute>(context, dto);
        }
        public static INamedTypeSymbol Controller(this GeneratorExecutionContext context, INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultControllerAttribute>(context, dto);
        }
        public static INamedTypeSymbol Repository(this GeneratorExecutionContext context, INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultRepositoryAttribute>(context, dto);
        }
        //For caching
        private static IEnumerable<INamedTypeSymbol> classSymbols;
        private static INamedTypeSymbol UserOrDefault<TAttribute>(this GeneratorExecutionContext context, INamedTypeSymbol dto, bool isUser = false)
            where TAttribute : Attribute
        {
            classSymbols = context.GetAllClasses("");
            var objectsWithAttribute = classSymbols.GetClassesWithAttribute(typeof(TAttribute).Name);
            if (dto.HasAttribute(nameof(UserDtoAttribute)))
            {
                return objectsWithAttribute.FirstOrDefault(x => x.HasAttribute(nameof(UserDtoAttribute)));
            }
            else
            {
                return objectsWithAttribute.FirstOrDefault(x => !x.HasAttribute(nameof(UserDtoAttribute)));
            }
        }
        public static string GetIdPropertyMethodeName(this INamedTypeSymbol dto)
        {
            INamedTypeSymbol baseType = dto;
            while (baseType is not null)
            {
                var result = baseType.GetMethodsWithAttribute(nameof(GetIdAttribute)).FirstOrDefault();
                if (result is not null)
                {
                    return result.Name+"()";
                }

                baseType = baseType.BaseType;
            }
            return null;
        }
        public static IPropertySymbol GetIdProperty(this INamedTypeSymbol dto)
        {
            INamedTypeSymbol baseType = dto;
            while (baseType is not null)
            {
                var result = baseType.GetPropertyWithAttribute(nameof(IdAttribute));
                if (result is not null)
                {
                    return result;
                }

                baseType = baseType.BaseType;
            }
            return null;
        }

        public static ClassWithMethods GetClassWithMethods(this INamedTypeSymbol classSymbol)
        {
            return new ClassWithMethods(classSymbol);
        }
    }
}
