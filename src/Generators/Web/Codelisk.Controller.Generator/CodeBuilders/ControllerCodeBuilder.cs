using CodeGenHelpers;
using Codelisk.GeneratorAttributes.Helper;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Models;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Controller.Generator.CodeBuilders
{
    public class ControllerCodeBuilder : BaseCodeBuilder
    {
        public ControllerCodeBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }

        public override List<CodeBuilder> Get(
            Compilation context,
            List<CodeBuilder> codeBuilders = null
        )
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(context);
            var dtos = attributeCompilationCrawler.Dtos().ToList();
            return Build(attributeCompilationCrawler, context, dtos);
        }

        private List<CodeBuilder?> Build(
            AttributeCompilationCrawler attributeCompilationCrawler,
            Compilation context,
            IEnumerable<INamedTypeSymbol> dtos
        )
        {
            var result = new List<CodeBuilder?>();
            foreach (var dto in dtos)
            {
                var manager = attributeCompilationCrawler.Manager(dto, CodeBuilderNamespace);
                var baseController = attributeCompilationCrawler.Controller(
                    dto,
                    CodeBuilderNamespace
                );
                var builder = CreateBuilder();

                var c = Class(builder, dto, manager, baseController, context);

                Methods(c, dto, baseController, manager.GetClassWithMethods());

                result.Add(builder);
            }

            return result;
        }

        private ClassBuilder Class(
            CodeBuilder builder,
            INamedTypeSymbol dto,
            INamedTypeSymbol manager,
            INamedTypeSymbol baseController,
            Compilation context
        )
        {
            var constructedBaseController = baseController.ConstructFromDto(dto, context);
            return builder
                .AddClass(dto.ControllerNameFromDto())
                .WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseController.GetFullTypeName())
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(
                    constructedBaseController,
                    (manager, dto.ManagerNameFromDto())
                )
                .Class;
        }

        private void Methods(
            ClassBuilder c,
            INamedTypeSymbol dto,
            INamedTypeSymbol baseController,
            ClassWithMethods repoModel
        )
        {
            var repoProperty = baseController
                .GetFieldsWithConstructedFromType(repoModel.Class)
                .First();

            Dictionary<Type, string> methodsWithControllerAttributeName =
                AttributeHelper.AllAttributesMethodeHeaderDictionary(
                    Constants.HttpDeleteAttribute,
                    Constants.HttpGetAttribute,
                    Constants.HttpPostAttribute
                );

            if (dto.HasAttribute(nameof(RemoveGetAll)))
            {
                methodsWithControllerAttributeName.Remove(typeof(GetAllAttribute));
            }

            foreach (var item in methodsWithControllerAttributeName)
            {
                if (
                    (
                        item.Key == typeof(GetFullAttribute)
                        || item.Key == typeof(GetAllFullAttribute)
                    ) && !dto.DtoHasForeignKeyAttribute()
                )
                {
                    continue;
                }
                var method = repoModel.MethodFromAttribute(item.Key);

                var httpAttribute = method.HttpAttribute();
                var methodBuilder = c.AddMethod(method.MethodName(dto), Accessibility.Public)
                    .AddAttribute(method.HttpControllerAttribute(dto, item.Value))
                    .WithReturnTypeForHttpMethod(item.Key, dto)
                    .AddParametersForHttpMethod(httpAttribute, dto);

                if (item.Key == typeof(GetAllFullAttribute) || item.Key == typeof(GetFullAttribute))
                {
                    methodBuilder.MakeAsync();
                }

                if (
                    item.Key == typeof(GetAllAttribute)
                    || item.Key == typeof(GetAllFullAttribute)
                    || item.Key == typeof(GetAttribute)
                    || item.Key == typeof(GetFullAttribute)
                )
                {
                    if (dto.HasAttribute(nameof(CustomizeGetAll)))
                    {
                        bool anonymous = dto.GetAttribute<CustomizeGetAll>()
                            .NamedArguments.Any(x => x.Key.Equals("AllowAnonymous"));
                        if (anonymous)
                        {
                            methodBuilder.AddAttribute(Constants.AllowAnonymousAttribute);
                        }
                    }
                }

                methodBuilder.WithBody(
                    (x) =>
                    {
                        var httpAttributeUrl = httpAttribute.AttributeUrl(dto);
                        var parameterNames = httpAttribute.GetParametersNamesForHttpMethod(dto);
                        var dtoFullName = dto.GetFullModelName();
                        if (item.Key == typeof(GetAllFullAttribute))
                        {
                            x.AppendLine(
                                $"var result = await {repoProperty.Name}.{httpAttributeUrl}({parameterNames});"
                            );
                            x.AppendLine($"return result.Cast<{dtoFullName}>().ToList();");
                            return;
                        }
                        else if (item.Key == typeof(GetFullAttribute))
                        {
                            x.AppendLine(
                                $"return (await {repoProperty.Name}.{httpAttributeUrl}({parameterNames})) as {dtoFullName};"
                            );
                            return;
                        }

                        x.AppendLine(
                            $"return {repoProperty.Name}.{httpAttributeUrl}({parameterNames});"
                        );
                    }
                );
            }
        }
    }
}
