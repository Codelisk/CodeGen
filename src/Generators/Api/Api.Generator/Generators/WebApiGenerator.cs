using System.Diagnostics;
using Api.Generator.Generators.CodeBuilders;
using CodeGenHelpers;
using Generators.Base;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;

namespace Api.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class WebApiGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var codebuilders = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var repositoriesCodeBuilder = new RepositoryCodeBuilder(
                        compilation.AssemblyName
                    ).Get(compilation);
                    var classServicesModuleInitializerBuilder = new ModuleInitializerBuilder(
                        compilation.AssemblyName
                    ).Get(compilation, repositoriesCodeBuilder);

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (repositoriesCodeBuilder, "Repositories", ("abstract partial", "partial")),
                        (classServicesModuleInitializerBuilder, null, null)
                    };

                    return result;
                }
            );

            AddSource(context, codebuilders);
        }
    }
}
