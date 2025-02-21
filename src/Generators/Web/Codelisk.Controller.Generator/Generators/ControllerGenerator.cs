using System.Collections.Immutable;
using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.Helper;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Codelisk.GeneratorShared.Constants;
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

            var combinedResults = baseDtos
                .Combine(dtos)
                .Combine(managers)
                .Combine(controllers)
                .Combine(base.DefaultNameSpace(context));

            context.RegisterImplementationSourceOutput(
                combinedResults,
                static (sourceProductionContext, combinedResult) =>
                {
                    var baseDtos = combinedResult.Left.Left.Left.Left; // baseDtos
                    var dtos = combinedResult.Left.Left.Left.Right; // dtos
                    var managers = combinedResult.Left.Left.Right; // managers
                    var controllers = combinedResult.Left.Right; // controllers
                    var defaultNamespace = combinedResult.Right; // DefaultNamespace

                    var result = new List<CodeBuilder?>();

                    foreach (var dto in dtos)
                    {
                        var baseManager = dto.TenantOrDefault<DefaultManagerAttribute>(managers);
                        var baseController = dto.TenantOrDefault<DefaultControllerAttribute>(
                            controllers
                        );

                        var builder = CodeBuilder.Create(defaultNamespace);
                        var c = Class(builder, dto, baseDtos, baseManager, baseController);

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
            ImmutableArray<RecordDeclarationSyntax> baseDtos,
            ClassDeclarationSyntax manager,
            ClassDeclarationSyntax baseController
        )
        {
            var constructedBaseController = baseController.Construct(dto);
            var result = builder
                .AddClass(dto.ControllerNameFromDto())
                .WithAccessModifier(Accessibility.Public)
                .SetBaseClass($"{baseController.Construct(dto).GetFullTypeName()}")
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .AddParameterWithBaseCall(
                    ("I" + dto.ManagerNameFromDto(), dto.ManagerNameFromDto().GetParameterName())
                )
                .Class;

            var foreignProperties = dto.DtoForeignProperties(baseDtos);

            foreach (var foreignProperty in foreignProperties)
            {
                result
                    .AddMethod($"GetBy{foreignProperty.GetPropertyName()}", Accessibility.Public)
                    .AddParameter("Guid", "id")
                    .AddAttribute(
                        $"[{Constants.HttpGetAttribute}(\"GetBy{foreignProperty.GetPropertyName()}\")]"
                    )
                    .MakeAsync()
                    .WithReturnTypeTaskList(dto.GetName())
                    .WithBody(x =>
                    {
                        x.AppendLine(
                            $"return await (_manager as I{dto.ManagerNameFromDto()}).GetBy{foreignProperty.GetPropertyName()}(id);"
                        );
                    });
            }

            if (foreignProperties.Any())
            {
                result
                    .AddMethod($"GetFull", Accessibility.Public)
                    .AddParameter("Guid", "id")
                    .AddAttribute($"[{Constants.HttpGetAttribute}(\"{ApiUrls.GetFull}\")]")
                    .MakeAsync()
                    .WithReturnTypeTask("Microsoft.AspNetCore.Mvc.IActionResult")
                    .WithBody(x =>
                    {
                        x.AppendLine(
                            $"return Ok(await (_manager as I{dto.ManagerNameFromDto()}).GetFull(id));"
                        );
                    });

                result
                    .AddMethod($"GetAllFull", Accessibility.Public)
                    .MakeAsync()
                    .AddAttribute($"[{Constants.HttpGetAttribute}(\"{ApiUrls.GetAllFull}\")]")
                    .WithReturnTypeTask("Microsoft.AspNetCore.Mvc.IActionResult")
                    .WithBody(x =>
                    {
                        x.AppendLine(
                            $"return Ok(await (_manager as I{dto.ManagerNameFromDto()}).GetAllFull());"
                        );
                    });
            }

            return result;
        }
    }
}
