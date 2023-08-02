using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Contract.Generator.Generators
{
    [Generator]
    public class RepositoryContractGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            Debugger.Launch(); 
            AddSource(context, "RepositoryContracts", new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context));
        }
    }
}
