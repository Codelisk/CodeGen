using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebDbContext.Generator.CodeBuilders;

namespace WebDbContext.Generator.Generators
{
    [Generator]
    public class DbContextGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {   
            var dbContextCodeBuilder = new DbContextCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var initializerBuilder = new DbContextInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, dbContextCodeBuilder);
            AddSource(context, "DbContexts", dbContextCodeBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}
