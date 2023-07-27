using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base;
using System.Xml.Linq;
using Foundation.Crawler;
using Attributes.WebAttributes.Repository.Base;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions;

namespace Controller.Generator
{
    public static class Extensions
    {
        public static string ControllerNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Controller";
        }
        public static string PostAttribute(this IMethodSymbol method, INamedTypeSymbol dto, bool plural = false)
        {
            return method.GetPropertyOfAttribute<BaseHttpAttribute, UrlAttribute>().AttributeUrl(dto, plural).AttributeWithConstructor(Constants.HttpPostAttribute);
        }
        public static string GetAttribute(this IMethodSymbol method, INamedTypeSymbol dto, bool plural = false)
        {
            return method.GetPropertyOfAttribute<BaseHttpAttribute, UrlAttribute>().AttributeUrl(dto, plural).AttributeWithConstructor(Constants.HttpGetAttribute);
        }
        public static string DeleteAttribute(this IMethodSymbol method, INamedTypeSymbol dto, bool plural = false)
        {
            return method.GetPropertyOfAttribute<BaseHttpAttribute, UrlAttribute>().AttributeUrl(dto, plural).AttributeWithConstructor(Constants.HttpDeleteAttribute);
        }

        public static string MethodName(this IMethodSymbol method, INamedTypeSymbol dto, bool plural = false)
        {
            return method.GetPropertyOfAttribute<BaseHttpAttribute, UrlAttribute>().AttributeUrl(dto, plural);
        }
    }
}
