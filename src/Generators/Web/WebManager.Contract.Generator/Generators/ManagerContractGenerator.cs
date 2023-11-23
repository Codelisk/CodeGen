using System.Diagnostics;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebManager.Contract.Generator.CodeBuilders;

namespace WebManager.Contract.Generator.Generators
{
    [Generator]
    public class ManagerContractGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            //Debugger.Launch();
            AddSource(context, "ManagerContracts", new ManagerContractCodeBuilder(context.Compilation.AssemblyName).Get(context));
        }
    }
}
