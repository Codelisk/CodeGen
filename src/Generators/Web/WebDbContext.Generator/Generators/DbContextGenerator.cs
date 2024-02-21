using CodeGenHelpers;
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
            var result = context.CompilationProvider.Select(static (compilation, _) =>
            {
                var codeBuilder = new DbContextCodeBuilder(compilation.AssemblyName).Get(compilation);
                var initializerBuilder = new DbContextInitializerCodeBuilder(compilation.AssemblyName).Get(compilation, codeBuilder);


                var result = new List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
                {
                    (codeBuilder, "DbContexts", null),
                    (initializerBuilder, null, null)
                };

                return result;
            });

            AddSourceFileName(context, result);
        }
    }
}
