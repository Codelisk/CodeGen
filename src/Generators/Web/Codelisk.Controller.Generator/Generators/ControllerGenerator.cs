
using CodeGenHelpers;
using Controller.Generator.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Controller.Generator.Generators
{
    [Generator]
    public class ControllerGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.CompilationProvider.Select(static (compilation, _) =>
            {
                var codeBuilder = new ControllerCodeBuilder(compilation.AssemblyName).Get(compilation);
                var initializerBuilder = new ControllerInitializerBuilder(compilation.AssemblyName).Get(compilation, codeBuilder);


                var result = new List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
                {
                    (codeBuilder, "Controller", null),
                    (initializerBuilder, null, null)
                };

                return result;
            });

            AddSource(context, result);
        }
    }
}
