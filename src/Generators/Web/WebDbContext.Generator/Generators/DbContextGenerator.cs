using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebDbContext.Generator.CodeBuilders;

namespace WebDbContext.Generator.Generators
{
    [Generator]
    public class DbContextGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            var dbContextCodeBuilder = new DbContextCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var initializerBuilder = new DbContextInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, dbContextCodeBuilder);
            AddSource(context, "DbContexts", dbContextCodeBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}
