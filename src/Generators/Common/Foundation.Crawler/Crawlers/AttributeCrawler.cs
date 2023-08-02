﻿using Attributes;
using Attributes.ApiAttributes;
using Attributes.WebAttributes.Controller;
using Attributes.WebAttributes.Database;
using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Manager;
using Attributes.WebAttributes.Repository;
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
        public static INamedTypeSymbol DefaultApiRepository(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(DefaultApiRepositoryAttribute)).First();
        }
        public static string AttributeUrl(this string attributeValue, INamedTypeSymbol dto, bool plural = false)
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return $"{attributeValue}";
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
            if(classSymbols is null)
            {
                classSymbols = context.GetAllClasses("");
            }
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
        public static IPropertySymbol GetIdProperty(this INamedTypeSymbol dto)
        {
            return dto.BaseType.GetPropertyWithAttribute(nameof(IdAttribute));
        }

        public static ClassWithMethods GetClassWithMethods(this INamedTypeSymbol classSymbol)
        {
            return new ClassWithMethods(classSymbol);
        }
    }
}
