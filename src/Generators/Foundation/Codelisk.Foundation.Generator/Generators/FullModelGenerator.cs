using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.Foundation.Generator.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;

namespace Codelisk.Foundation.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class FullModelGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var refitApiCodeBuilder = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    var codeBuilder = new FullModelCodeBuilder(compilation.AssemblyName).Get(
                        compilation
                    );

                    var result = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (codeBuilder, "FullModels", ("namespace <global namespace>;", "")),
                    };

                    return result;
                }
            );

            AddSource(context, refitApiCodeBuilder);
        }
    }
}
