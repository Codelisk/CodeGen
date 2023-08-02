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
            var initializerBuilder = new ManagerInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, managerCodeBuilder);
            AddSource(context, "Manager", managerCodeBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}
