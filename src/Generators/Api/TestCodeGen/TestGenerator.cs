using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ClassLibrary1.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var codebuilders = context.CompilationProvider.Select(
                static (compilation, _) =>
                {
                    return "";
                }
            );

            // generate a class that contains their values as const strings
            context.RegisterSourceOutput(
                codebuilders,
                static (sourceProductionContext, codeBuildersTuples) =>
                {
                    sourceProductionContext.AddSource(
                        $"Test2.cs",
                        $@"
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{{
    internal class Test
    {{
    }}
}}
"
                    );
                }
            );
        }
    }
}
