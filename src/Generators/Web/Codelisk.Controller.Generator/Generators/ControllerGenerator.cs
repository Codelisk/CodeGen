using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Controller.Generator.CodeBuilders;
using Foundation.Crawler.Crawlers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;

namespace Controller.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class ControllerGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var codeBuilder = new ControllerCodeBuilder(compilation.AssemblyName).Get(
                        compilation
                    );

                    var initializerBuilder = new ControllerInitializerBuilder(
                        new AttributeCompilationCrawler(
                            compilation
                        ).NameSpaceAndMethod<ControllerModuleInitializerAttribute>()
                    ).Get(compilation, codeBuilder);

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (codeBuilder, "Controller", null),
                        (initializerBuilder, null, null)
                    };

                    return result;
                }
            );

            AddSource(context, result);
        }
    }
}
