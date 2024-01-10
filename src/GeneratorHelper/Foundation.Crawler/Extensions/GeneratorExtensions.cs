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
            else if(httpAttribute.HasAttribute(nameof(DtoBodyListAttribute)))
            {
                return dto.Name.GetParameterName(true);
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

            if (httpAttribute.HasAttribute(nameof(DtoBodyListAttribute)))
            {
                methodBuilder.AddParameter($"System.Collections.Generic.List<{dto.Name}>", dto.Name.GetParameterName(true));
            }

            return methodBuilder;
        }
        
        public static MethodBuilder WithReturnTypeForHttpMethod(this MethodBuilder methodBuilder, Type httpAttribute, INamedTypeSymbol dto)
        {
            var returnAttributeValue = httpAttribute.GetRealAttributeFromAttribute<ReturnAttribute>()?.ReturnKind;

            if(returnAttributeValue is null)
            {
                return methodBuilder.WithReturnTypeTask();
            }

            if (returnAttributeValue == ReturnKind.List)
            {
                methodBuilder.WithReturnTypeTaskList(dto.Name);
            }
            else if(returnAttributeValue == ReturnKind.Model)
            {
                methodBuilder.WithReturnTypeTask(dto.Name);
            }
            else if (returnAttributeValue == ReturnKind.ListFull)
            {
                methodBuilder.WithReturnTypeTask($"List<{dto.GetFullModelName()}>");
            }
            else if(returnAttributeValue == ReturnKind.ModelFull)
            {
                methodBuilder.WithReturnTypeTask($"{dto.GetFullModelName()}");
            }
            else
            {
                return methodBuilder.WithReturnTypeTask();
            }

            return methodBuilder;
        }
    }
}
