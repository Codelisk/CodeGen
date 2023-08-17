using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Contract.Generator.CodeBuilders
{
    public class RepositoryContractCodeBuilder : BaseCodeBuilder
    {
        public RepositoryContractCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var repos = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);

            List<CodeBuilder> result = new List<CodeBuilder>();
            foreach (var repo in repos)
            {
                var interfaceBuilder = ClassInterface(repo, context);
                result.Add(interfaceBuilder);
            }

            return result;
        }
        private CodeBuilder ClassInterface(CodeBuilder originalBuilder, GeneratorExecutionContext context)
        {
            return originalBuilder.GenerateSeperateInterfaceCodeBuilder<RegisterTransient>(context);
        }
    }
}
