using Controller.Generator.Generators.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators
{
    [Generator]
    public class DbContextGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var dbContextCodeBuilder = new DbContextCodeBuilder().Get(context);
            AddSource(context, "DbContexts", dbContextCodeBuilder);
        }
    }
}
