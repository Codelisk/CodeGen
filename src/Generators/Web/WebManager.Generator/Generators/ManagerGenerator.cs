using System.Diagnostics;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebManager.Generator.CodeBuilders;

namespace WebManager.Generator.Generators
{
    [Generator]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Execute(Compilation compilation)
        {
            if (compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            try
            {
                //Debugger.Launch();
                var managerCodeBuilder = new ManagerCodeBuilder(compilation.AssemblyName).Get(compilation);
                var initializerBuilder = new ManagerInitializerCodeBuilder(compilation.AssemblyName).Get(compilation, managerCodeBuilder);
                AddSource(compilation, "Manager", managerCodeBuilder);
                AddSource(compilation, "", initializerBuilder);
            }
            catch(Exception ex)
            {
                context.AddSource("ErrorManagerGenerator", $"//{ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }
}
