using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using WebManager.Contract.Generator.CodeBuilders;

namespace WebManager.Contract.Generator.Generators
{
    [Generator]
    public class ManagerContractGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            AddSource(context, "ManagerContracts", new ManagerContractCodeBuilder(context.Compilation.AssemblyName).Get(context));
        }
    }
}
