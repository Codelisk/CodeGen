using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using WebManager.Generator.CodeBuilders;

namespace WebManager.Generator.Generators
{
    [Generator]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var managerCodeBuilder = new ManagerCodeBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "Manager", managerCodeBuilder);
        }
    }
}
