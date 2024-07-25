using System.Collections.Generic;
using CodeGenHelpers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebRepositories.Contract.Generator.CodeBuilders;

namespace WebRepositories.Contract.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class RepositoryContractGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var repositoryContractCodeBuilder = new RepositoryContractCodeBuilder(
                        compilation.AssemblyName
                    ).Get(compilation);

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (repositoryContractCodeBuilder, "RepositoryContracts", null),
                    };

                    return result;
                }
            );

            AddSource(context, result);
        }
    }
}
