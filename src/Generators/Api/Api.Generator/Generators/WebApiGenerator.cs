using Api.Generator.Generators.CodeBuilders;
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
            context.AddSource("Test.cs", "TEST");
            return;
            var refitApiCodeBuilder = new RefitApiCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var repositoriesCodeBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var classServicesModuleInitializerBuilder = new ModuleInitializerBuilder(context.Compilation.AssemblyName).Get(context, repositoriesCodeBuilder);

            AddSource(context, "Repositories", repositoriesCodeBuilder, ("abstract partial", "partial"));
            AddSource(context, "Apis", refitApiCodeBuilder, ("abstract partial", "partial"));
            AddSource(context, "", classServicesModuleInitializerBuilder);
            //AddSource(context, "Repositories", repositoriesCodeBuilder);
        }
    }
}
