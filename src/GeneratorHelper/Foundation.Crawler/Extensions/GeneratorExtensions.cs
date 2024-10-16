using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorShared.Models;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.New;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.Extensions
{
    public static class GeneratorExtensions
    {
        public static ClassBuilder AddDtoUsing(
            this ClassBuilder classBuilder,
            AttributeCompilationCrawler context
        )
        {
            var namespaces = context.Dtos().Select(x => x.GetNamespace()).Distinct();
            foreach (var n in namespaces)
            {
                classBuilder.AddNamespaceImport(n);
            }

            return classBuilder;
        }

        public static string GetParametersNamesForHttpMethod(
            this INamedTypeSymbol httpAttribute,
            INamedTypeSymbol dto
        )
        {
            if (httpAttribute.HasAttribute(nameof(IdQueryAttribute)))
            {
                return dto.GetIdProperty().Name.GetParameterName();
            }

            if (httpAttribute.HasAttribute(nameof(DtoBodyAttribute)))
            {
                return dto.Name.GetParameterName();
            }
            else if (httpAttribute.HasAttribute(nameof(DtoBodyListAttribute)))
            {
                return dto.Name.GetParameterName(true);
            }

            return string.Empty;
        }

        public static MethodBuilder AddParametersForHttpMethod(
            this MethodBuilder methodBuilder,
            INamedTypeSymbol httpAttribute,
            INamedTypeSymbol dto
        )
        {
            if (httpAttribute.HasAttribute(nameof(IdQueryAttribute)))
            {
                methodBuilder.AddParameter(
                    dto.GetIdProperty().Type.Name,
                    dto.GetIdProperty().Name.GetParameterName()
                );
            }

            if (httpAttribute.HasAttribute(nameof(DtoBodyAttribute)))
            {
                methodBuilder.AddParameter(dto.GetFullTypeName(), dto.Name.GetParameterName());
            }

            if (httpAttribute.HasAttribute(nameof(DtoBodyListAttribute)))
            {
                methodBuilder.AddParameter(
                    $"System.Collections.Generic.List<{dto.GetFullTypeName()}>",
                    dto.Name.GetParameterName(true)
                );
            }

            return methodBuilder;
        }

        public static MethodBuilder AddParametersForHttpMethod(
            this MethodBuilder methodBuilder,
            IEnumerable<AttributeSyntax> httpAttributes,
            RecordDeclarationSyntax dto,
            IEnumerable<RecordDeclarationSyntax> dtos
        )
        {
            foreach (var httpAttribute in httpAttributes)
            {
                if (httpAttribute.HasAttribute<IdQueryAttribute>())
                {
                    methodBuilder.AddParameter(
                        dto.GetIdProperty(dtos).GetPropertyType(),
                        dto.GetIdProperty(dtos).GetPropertyName().GetParameterName()
                    );
                }

                if (httpAttribute.HasAttribute<DtoBodyAttribute>())
                {
                    methodBuilder.AddParameter(
                        dto.GetFullTypeName(),
                        dto.GetName().GetParameterName()
                    );
                }

                if (httpAttribute.HasAttribute<DtoBodyListAttribute>())
                {
                    methodBuilder.AddParameter(
                        $"System.Collections.Generic.List<{dto.GetFullTypeName()}>",
                        dto.GetName().GetParameterName(true)
                    );
                }
            }

            return methodBuilder;
        }

        public static MethodBuilder WithReturnTypeForHttpMethod(
            this MethodBuilder methodBuilder,
            Type httpAttribute,
            INamedTypeSymbol dto
        )
        {
            var returnAttributeValue = httpAttribute
                .GetRealAttributeFromAttribute<ReturnAttribute>()
                ?.ReturnKind;

            if (returnAttributeValue is null)
            {
                return methodBuilder.WithReturnTypeTask();
            }

            if (returnAttributeValue == ReturnKind.List)
            {
                methodBuilder.WithReturnTypeTaskList(dto.GetFullTypeName());
            }
            else if (returnAttributeValue == ReturnKind.Model)
            {
                methodBuilder.WithReturnTypeTask(dto.GetFullTypeName());
            }
            else if (returnAttributeValue == ReturnKind.ModelNullable)
            {
                methodBuilder.WithReturnTypeTask(dto.GetFullTypeName() + "?");
            }
            else if (returnAttributeValue == ReturnKind.ListFull)
            {
                methodBuilder.WithReturnTypeTask($"List<{dto.GetFullModelName()}>");
            }
            else if (returnAttributeValue == ReturnKind.ModelFull)
            {
                methodBuilder.WithReturnTypeTask($"{dto.GetFullModelName()}");
            }
            else
            {
                return methodBuilder.WithReturnTypeTask();
            }

            return methodBuilder;
        }

        public static MethodBuilder WithReturnTypeForHttpMethod(
            this MethodBuilder methodBuilder,
            Type httpAttribute,
            RecordDeclarationSyntax dto
        )
        {
            var returnAttributeValue = httpAttribute
                .GetRealAttributeFromAttribute<ReturnAttribute>()
                ?.ReturnKind;

            if (returnAttributeValue is null)
            {
                return methodBuilder.WithReturnTypeTask();
            }

            if (returnAttributeValue == ReturnKind.List)
            {
                methodBuilder.WithReturnTypeTaskList(dto.GetFullTypeName());
            }
            else if (returnAttributeValue == ReturnKind.Model)
            {
                methodBuilder.WithReturnTypeTask(dto.GetFullTypeName());
            }
            else if (returnAttributeValue == ReturnKind.ModelNullable)
            {
                methodBuilder.WithReturnTypeTask(dto.GetFullTypeName() + "?");
            }
            else if (returnAttributeValue == ReturnKind.ListFull)
            {
                methodBuilder.WithReturnTypeTask($"List<{dto.GetFullModelName()}>");
            }
            else if (returnAttributeValue == ReturnKind.ModelFull)
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
