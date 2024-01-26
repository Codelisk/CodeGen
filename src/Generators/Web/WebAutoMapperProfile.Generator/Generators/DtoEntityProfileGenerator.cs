using CodeGenHelpers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using WebAutoMapperProfile.Generator.CodeBuilders;

namespace WebAutoMapperProfile.Generator.Generators
{
    [Generator]
    public class DtoEntityProfileGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(static (compilation, _) =>
            {
                var repositoryContractCodeBuilder = new DtoEntityProfileBuilder(compilation.AssemblyName).Get(compilation);

                var result = new List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
                {
                    (repositoryContractCodeBuilder, "AutoMapper", null)
                };

                return result;
            });

            AddSource(context, result);
        }
    }
}
