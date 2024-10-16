using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.Helper;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Controller.Generator.CodeBuilders;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Extensions.New;
using Foundation.Crawler.Extensions.New.AttributeFinder;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebGenerator.Base;

namespace Controller.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class ControllerGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var managers = context.DefaultManagers();
            var controllers = context.DefaultControllers();
            var baseDtos = context.BaseDtos();

            var combinedResults = baseDtos.Combine(dtos).Combine(managers).Combine(controllers);

            context.RegisterImplementationSourceOutput(
                combinedResults,
                static (sourceProductionContext, combinedResult) =>
                {
                    var baseDtos = combinedResult.Left.Left.Left; // baseDtos
                    var dtos = combinedResult.Left.Left.Right; // dtos
                    var managers = combinedResult.Left.Right; // managers
                    var controllers = combinedResult.Right;

                    var result = new List<CodeBuilder?>();

                    foreach (var dto in dtos)
                    {
                        var baseManager = dto.TenantOrDefault<DefaultManagerAttribute>(managers);
                        var baseController = dto.TenantOrDefault<DefaultControllerAttribute>(
                            controllers
                        );

                        var builder = CodeBuilder.Create(dto.GetNamespace());
                        var c = Class(builder, dto, baseManager, baseController);

                        Methods(c, dto, baseController, baseManager.GetClassWithMethods(managers));

                        result.Add(builder);
                    }

                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "Controller", null),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static ClassBuilder Class(
            CodeBuilder builder,
            RecordDeclarationSyntax dto,
            ClassDeclarationSyntax manager,
            ClassDeclarationSyntax baseController
        )
        {
            var constructedBaseController = baseController.Construct(dto);
            return builder
                .AddClass(dto.ControllerNameFromDto())
                .WithAccessModifier(Accessibility.Public)
                .SetBaseClass(
                    $"{baseController.GetName()}<{dto.GetName()}, Guid, {dto.GetEntityName()}>"
                )
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseController)
                .Class;
        }

        private static void Methods(
            ClassBuilder c,
            RecordDeclarationSyntax dto,
            ClassDeclarationSyntax baseController,
            ClassWithMethods repoModel
        )
        {
            var repoProperty = baseController
                .GetFieldsWithConstructedFromType(repoModel.Class)
                .First();

            Dictionary<Type, string> methodsWithControllerAttributeName =
                AttributeHelper.AllFullAttributesMethodeHeaderDictionary(
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
