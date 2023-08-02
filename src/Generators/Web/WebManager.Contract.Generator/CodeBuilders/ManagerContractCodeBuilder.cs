using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManager.Generator.CodeBuilders;
using Generators.Base.Helpers;
using Attributes.GeneralAttributes.Registration;

namespace WebManager.Contract.Generator.CodeBuilders
{
    public class ManagerContractCodeBuilder : BaseCodeBuilder
    {
        public ManagerContractCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var repos = new ManagerCodeBuilder(context.Compilation.AssemblyName).Get(context);

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
