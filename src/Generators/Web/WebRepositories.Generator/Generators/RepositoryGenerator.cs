﻿using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebRepositories.Generator.CodeBuilders;

namespace WebRepositories.Generator.Generators
{
    [Generator]
    public class RepositoryGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var repositoryBuilder = new RepositoryCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var initializerBuilder = new RepositoryInitializerCodeBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "Repositories", repositoryBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}