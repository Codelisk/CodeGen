using Microsoft.CodeAnalysis;
using Api.Generator.Generators.CodeBuilders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Generators.Base.Generators.Base;

namespace Api.Generator.Generators
{
    //[Generator]
    public class WebApiGenerator : BaseGenerator, ISourceGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();
            var refitApiCodeBuilder = new RefitApiCodeBuilder(context.Compilation.AssemblyName).Get(context);
            //var repositoriesCodeBuilder = new RepositoriesCodeBuilder(context.Compilation.AssemblyName).Get(context, refitApiCodeBuilder);
                   
            AddSource(context, "Apis", refitApiCodeBuilder, ("abstract partial", "partial"));
            //AddSource(context, "Repositories", repositoriesCodeBuilder);
        }
    }
}
