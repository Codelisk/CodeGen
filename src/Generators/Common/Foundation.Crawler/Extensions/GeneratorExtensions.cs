using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Shared.Models;

namespace Foundation.Crawler.Extensions.Extensions
{
    public static class GeneratorExtensions
    {
        public static ClassBuilder AddDtoUsing(this ClassBuilder classBuilder, GeneratorExecutionContext context)
        {
            var namespaces = context.Dtos().Select(x => x.GetNamespace()).Distinct();
            foreach (var n in namespaces)
            {
                classBuilder.AddNamespaceImport(n);
            }

            return classBuilder;
        }
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
            if (httpAttribute.HasAttribute(nameof(IdQueryAttribute)))
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

            var returnAttributeValue = returnAttribute.GetFirstConstructorArgumentEnum<ReturnKind>();

            if (returnAttributeValue == ReturnKind.List)
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
