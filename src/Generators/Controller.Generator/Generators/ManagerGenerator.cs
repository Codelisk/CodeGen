using Controller.Generator.Generators.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators
{
    [Generator]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var managerCodeBuilder = new ManagerCodeBuilder().Get(context);
            AddSource(context, "Manager", managerCodeBuilder);
        }
    }
}
