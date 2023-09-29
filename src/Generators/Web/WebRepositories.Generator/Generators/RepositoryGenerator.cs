﻿using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
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
            //Debugger.Launch();
            var repositoryBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var initializerBuilder = new RepositoryInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context, repositoryBuilder);
            AddSource(context, "Repositories", repositoryBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}
