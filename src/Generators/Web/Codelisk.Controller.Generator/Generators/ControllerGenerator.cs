using System.Collections.Immutable;
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

                        var builder = CodeBuilder.Create("Communalaudit.Api");
                        var c = Class(builder, dto, baseManager, baseController);

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
    }
}
