using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebDbContext.Generator.CodeBuilders;

namespace WebDbContext.Generator.Generators
{
    [Generator]
    public class DbContextGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var codeBuilder = new DbContextCodeBuilder(compilation.AssemblyName).Get(
                        compilation
                    );
                    var initializerBuilder = new DbContextInitializerCodeBuilder(
                        new AttributeCompilationCrawler(
                            compilation
                        ).GetInitNamespace<DatabaseModuleInitializerAttribute>()
                    ).Get(compilation, codeBuilder);

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (codeBuilder, "DbContexts", null),
                        (initializerBuilder, null, null)
                    };

                    return result;
                }
            );

            AddSource(context, result);
        }
    }
}
