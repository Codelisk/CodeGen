using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Foundation.Crawler.Extensions.New;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions
{
    public static class AttributeFinderExtensions
    {
        public static string AttributeUrl(this string attributeValue, INamedTypeSymbol dto)
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return $"{attributeValue}";
        }

        public static string AttributeUrl(
            this INamedTypeSymbol attributeSymobl,
            INamedTypeSymbol dto
        )
        {
            var urlProperty = attributeSymobl.GetAttribute<UrlAttribute>();
            bool plural = attributeSymobl.HasAttribute(nameof(PluralAttribute));
            return urlProperty.GetFirstConstructorArgument().AttributeUrl(dto);
        }

        public static string GetIdPropertyMethodeName(this INamedTypeSymbol dto)
        {
            INamedTypeSymbol baseType = dto;
            while (baseType is not null)
            {
                var result = baseType
                    .GetMethodsWithAttribute(nameof(GetIdAttribute))
                    .FirstOrDefault();
                if (result is not null)
                {
                    return result.Name + "()";
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

        public static PropertyDeclarationSyntax GetIdProperty(
            this RecordDeclarationSyntax dto,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            return dto.DtoProperties(baseDtos).First(x => x.HasAttribute(nameof(IdAttribute)));
        }

        public static ClassWithMethods GetClassWithMethods(
            this ClassDeclarationSyntax classSymbol,
            ImmutableArray<ClassDeclarationSyntax> baseTypes
        )
        {
            return new ClassWithMethods(classSymbol, baseTypes);
        }
    }
}
