using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebRepositories.Contract.Generator.CodeBuilders;

namespace WebRepositories.Contract.Generator.Generators
{
    [Generator]
    public class RepositoryContractGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            AddSource(context, "RepositoryContracts", new RepositoryContractCodeBuilder(context.Compilation.AssemblyName).Get(context));
        }
    }
}
