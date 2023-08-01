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
            Debugger.Launch();
            var dbContextCodeBuilder = new DbContextCodeBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "DbContexts", dbContextCodeBuilder);
        }
    }
}
