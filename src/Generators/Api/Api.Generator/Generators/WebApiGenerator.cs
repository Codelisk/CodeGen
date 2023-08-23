using Api.Generator.Generators.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Api.Generator.Generators
{
    [Generator]
    public class WebApiGenerator : BaseGenerator
    {
        public static string line = "";
        public override void Execute(GeneratorExecutionContext context)
        {
            try
            {
                var refitApiCodeBuilder = new RefitApiCodeBuilder(context.Compilation.AssemblyName).Get(context);
                line = "1";
                var repositoriesCodeBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
                line = "2";
                var classServicesModuleInitializerBuilder = new ModuleInitializerBuilder(context.Compilation.AssemblyName).Get(context, repositoriesCodeBuilder);

                AddSource(context, "Repositories", repositoriesCodeBuilder, ("abstract partial", "partial"));
                AddSource(context, "Apis", refitApiCodeBuilder, ("abstract partial", "partial"));
                AddSource(context, "", classServicesModuleInitializerBuilder);
                //AddSource(context, "Repositories", repositoriesCodeBuilder);
            }
            catch (Exception ex)
            {
                context.AddSource("Test.cs", "LINE " + line + ex.Message + " INNER:" + ex.InnerException?.Message + "\n" + ex.StackTrace.ToString());
            }
        }
    }
}
