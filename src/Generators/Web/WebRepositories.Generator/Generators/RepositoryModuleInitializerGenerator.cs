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

namespace WebRepositories.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class RepositoryModuleInitializerGenerator : BaseModuleInitializerGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            context.RegisterImplementationSourceOutput(
                dtos.Combine(base.DefaultNameSpace(context)),
                static (sourceProductionContext, combined) =>
                {
                    var result = new List<CodeBuilder?>();
                    var builder = CodeBuilder.Create(combined.Right);
                    List<(
                        string serviceUsage,
                        string serviceType,
                        string serviceImplementation
                    )> services = new();
                    foreach (var dto in combined.Left)
                    {
                        services.Add(
                            (null, "I" + dto.RepositoryNameFromDto(), dto.RepositoryNameFromDto())
                        );
                    }

                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (Get(builder, services, "AddRepositoryServices"), "Repositories", null)
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }
    }
}
