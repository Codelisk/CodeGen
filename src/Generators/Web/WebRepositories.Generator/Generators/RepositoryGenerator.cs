using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
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
                    var crawler = new AttributeCompilationCrawler(compilation);

                    var initializerBuilder = new RepositoryInitializerCodeBuilder(
                        crawler.NameSpaceAndMethod<RepositoryModuleInitializerAttribute>()
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
