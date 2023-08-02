using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebRepositories.Contract.Generator.CodeBuilders;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Contract.Generator.Generators
{
    [Generator]
    public class RepositoryContractGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            AddSource(context, "RepositoryContracts", new RepositoryContractCodeBuilder(context.Compilation.AssemblyName).Get(context));
        }
    }
}
