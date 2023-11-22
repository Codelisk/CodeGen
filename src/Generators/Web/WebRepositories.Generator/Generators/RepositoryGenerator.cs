using System.Diagnostics;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Generator.Generators
{
    [Generator]
    public class RepositoryGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            try
            {
                var repositoryBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
                var initializerBuilder = new RepositoryInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, repositoryBuilder);
                AddSource(context, "Repositories", repositoryBuilder);
                AddSource(context, "", initializerBuilder);
            }
            catch(Exception ex)
            {
                context.AddSource("ErrorRepositoryGenerator", $"//{ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }
}
