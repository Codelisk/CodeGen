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
            try
            {
                var dbContextCodeBuilder = new DbContextCodeBuilder(context.Compilation.AssemblyName).Get(context);
                var initializerBuilder = new DbContextInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, dbContextCodeBuilder);
                AddSource(context, "DbContexts", dbContextCodeBuilder);
                AddSource(context, "", initializerBuilder);
            }
            catch (Exception ex)
            {
                context.AddSource("ErrorDbContextGenerator", $"//{ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }
}
