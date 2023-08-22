﻿using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.CodeBuilders
{
    public abstract class ServicesModuleInitializerBuilder : BaseModuleInitializerBuilder
    {
        public ServicesModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            if (Services is null)
            {
                Services = new List<(string, string, string)>();
            }

            foreach (var codeBuilder in codeBuilders)
            {
                foreach (var c in codeBuilder.GetClasses(context))
                {
                    Services.Add(("AddSingleton", c.Interfaces.First().Name, c.Name));
                }
            }

            return base.Get(context);
        }
    }
}