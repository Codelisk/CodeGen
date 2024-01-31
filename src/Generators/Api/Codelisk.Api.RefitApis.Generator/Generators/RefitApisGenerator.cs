using Api.RefitApis.Generator.CodeBuilders;
using CodeGenHelpers;
using Generators.Base.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.RefitApis.Generator.Generators
{
    [Generator]
    public class RefitApisGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var refitApiCodeBuilder = context.CompilationProvider.Select(static (compilation, _) =>
            {
                var refitApiCodeBuilder = new RefitApiCodeBuilder(compilation.AssemblyName).Get(compilation);

                var result = new List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
                {
                    (refitApiCodeBuilder, "Apis", ("abstract partial", "partial")),
                };

                return result;
            });

            AddSourceImplementation(context, refitApiCodeBuilder);
        }
    }
}
