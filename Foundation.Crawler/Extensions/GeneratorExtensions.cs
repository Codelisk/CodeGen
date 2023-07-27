using Attributes.WebAttributes.Repository;
using Attributes.WebAttributes.Repository.Base;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Crawler.Extensions.Extensions
{
    public static class GeneratorExtensions
    {

        public static string GetParametersNamesForHttpMethod(this INamedTypeSymbol httpAttribute, INamedTypeSymbol dto)
        {
            if (httpAttribute.HasAttribute(nameof(IdQueryAttribute)))
            {
                return dto.GetIdProperty().Name.GetParameterName();
            }

            if (httpAttribute.HasAttribute(nameof(DtoBodyAttribute)))
            {
                return dto.Name.GetParameterName();
            }

            return string.Empty;
        }
        public static MethodBuilder AddParametersForHttpMethod(this MethodBuilder methodBuilder, INamedTypeSymbol httpAttribute, INamedTypeSymbol dto)
        {
            if(httpAttribute.HasAttribute(nameof(IdQueryAttribute)))
            {
                methodBuilder.AddParameter(dto.GetIdProperty().Type.Name, dto.GetIdProperty().Name.GetParameterName());
            }

            if (httpAttribute.HasAttribute(nameof(DtoBodyAttribute)))
            {
                methodBuilder.AddParameter(dto.Name, dto.Name.GetParameterName());
            }

            return methodBuilder;
        }
        public static MethodBuilder WithReturnTypeForHttpMethod(this MethodBuilder methodBuilder, INamedTypeSymbol httpAttribute, INamedTypeSymbol dto)
        {
            if (!httpAttribute.HasAttribute(nameof(ReturnAttribute)))
            {
                return methodBuilder.WithReturnTypeTask();
            }

            var returnAttribute = httpAttribute.GetAttribute<ReturnAttribute>();

            var returnAttributeValue = returnAttribute.GetFirstConstructorArgument<ReturnKind>();

            if(returnAttributeValue == ReturnKind.List)
            {
                methodBuilder.WithReturnTypeTaskList(dto.Name);
            }
            else
            {
                methodBuilder.WithReturnTypeTask(dto.Name);
            }

            return methodBuilder;
        }
    }
}
