using System.Diagnostics;
using CodeGenHelpers;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebManager.Generator.CodeBuilders;

namespace WebManager.Generator.Generators
{
    [Generator]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(static (compilation, _) =>
            {
                var codeBuilder = new ManagerCodeBuilder(compilation.AssemblyName).Get(compilation);
                var initializerBuilder = new ManagerInitializerCodeBuilder(compilation.AssemblyName).Get(compilation, codeBuilder);


                var result = new List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
                {
                    (codeBuilder, "Manager", null),
                    (initializerBuilder, null, null)
                };

                return result;
            });

            AddSourceFileName(context, result);
        }
    }
}
