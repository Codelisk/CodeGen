using Api.Generator.Generators.CodeBuilders;
using Generators.Base;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Api.Generator.Generators
{
    [Generator]
    public class WebApiGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            try
            {
                if (context.Compilation.AssemblyName.Contains("Generator"))
                {
                    return;
                }
                //Debugger.Launch();
                var repositoriesCodeBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
                var classServicesModuleInitializerBuilder = new ModuleInitializerBuilder(context.Compilation.AssemblyName, "AddApis").Get(context, repositoriesCodeBuilder);

                AddSource(context, "Repositories", repositoriesCodeBuilder, ("abstract partial", "partial"));
                AddSource(context, "", classServicesModuleInitializerBuilder);
                //AddSource(context, "Repositories", repositoriesCodeBuilder);
            }
            catch (Exception ex)
            {
                context.AddSource("Test.cs", "LOG:" + TestLog.Log + ex.Message + ":: INNER:" + ex.InnerException?.Message + "\n" + ex.StackTrace.ToString());
            }
        }
    }
}
