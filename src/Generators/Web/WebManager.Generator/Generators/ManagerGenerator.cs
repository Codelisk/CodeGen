using System.Diagnostics;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebManager.Generator.CodeBuilders;

namespace WebManager.Generator.Generators
{
    [Generator]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            try
            {
                //Debugger.Launch();
                var managerCodeBuilder = new ManagerCodeBuilder(context.Compilation.AssemblyName).Get(context);
                var initializerBuilder = new ManagerInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, managerCodeBuilder);
                AddSource(context, "Manager", managerCodeBuilder);
                AddSource(context, "", initializerBuilder);
            }
            catch(Exception ex)
            {
                context.AddSource("ErrorManagerGenerator", $"//{ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }
}
