using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Generator.Generators
{
    [Generator]
    public class RepositoryGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var codeBuilder = new RepositoryCodeBuilder(compilation.AssemblyName).Get(
                        compilation
                    );
                    var initializerBuilder = new RepositoryInitializerCodeBuilder(
                        new AttributeCompilationCrawler(
                            compilation
                        ).GetInitNamespace<RepositoryModuleInitializerAttribute>()
                    ).Get(compilation, codeBuilder);

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (codeBuilder, "Repositories", null),
                        (initializerBuilder, null, null)
                    };

                    return result;
                }
            );

            AddSource(context, result);
        }
    }
}
