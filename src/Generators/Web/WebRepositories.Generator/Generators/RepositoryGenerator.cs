using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.New;
using Foundation.Crawler.Extensions.New.AttributeFinder;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebGenerator.Base;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class RepositoryGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var repos = context.DefaultRepositories();
            var baseDtosAndClasses = context.BaseDtos().Combine(dtos).Combine(repos);
            context.RegisterImplementationSourceOutput(
                baseDtosAndClasses,
                static (sourceProductionContext, combinedResult) =>
                {
                    // Hier kannst du die kombinierten Ergebnisse verarbeiten
                    var (baseDtos, dtos) = combinedResult.Left;
                    var repos = combinedResult.Right;

                    var result = new List<CodeBuilder?>();
                    foreach (var dto in dtos)
                    {
                        var builder = CodeBuilder.Create("Communalaudit.Api");
                        var repo = dto.TenantOrDefault<DefaultRepositoryAttribute>(repos);
                        Class(builder, dto, repo, baseDtos);
                        result.Add(builder);
                    }
                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "Repositories", null),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static ClassBuilder Class(
            CodeBuilder builder,
            RecordDeclarationSyntax dto,
            ClassDeclarationSyntax baseRepository,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            var constructedBaseRepo = baseRepository.Construct(dto);

            builder
                .AddClass("I" + dto.RepositoryNameFromDto())
                .OfType(TypeKind.Interface)
                .SetBaseClass(
                    dto.ReplaceConstructValue(constructedBaseRepo.GetFirstInterfaceFullTypeName())
                )
                .WithAccessModifier(Accessibility.Public);

            return builder
                .AddClass(dto.RepositoryNameFromDto())
                .WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.RepositoryNameFromDto())
                .SetBaseClass($"{constructedBaseRepo.GetName()}<{dto.GetEntityName()}, Guid>")
                .AddAttribute(typeof(GeneratedRepositoryAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseRepo, ("TKey", "Guid"))
                .Class;
        }
    }
}
