using Api.RefitApis.Generator.CodeBuilders;
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
        public override void Execute(GeneratorExecutionContext context)
        {
            var refitApiCodeBuilder = new RefitApiCodeBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "Apis", refitApiCodeBuilder, ("abstract partial", "partial"));
        }
    }
}
