using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base;
using System.Xml.Linq;
using Attributes.WebAttributes.Repository.Base;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions;
using Foundation.Crawler.Extensions;

namespace Controller.Generator
{
    public static class Extensions
    {
        public static string ControllerNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Controller";
        }
        public static string HttpControllerAttribute(this IMethodSymbol method, INamedTypeSymbol dto, string httpControllerAttribute)
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
    }
}
