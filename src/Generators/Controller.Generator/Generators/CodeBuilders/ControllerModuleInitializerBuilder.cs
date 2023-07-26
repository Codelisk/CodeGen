using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators.CodeBuilders
{
    public class ControllerModuleInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ControllerModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        { 
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            Services = new List<(string, string, string)>();
            var baseContext = context.BaseContext();
            foreach (var dto in context.Dtos())
            {
                Services.Add(("AddDbContext", baseContext.Construct(dto).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), string.Empty));
            }

            return base.Get(context, codeBuilders);
        }

        public override string ModuleName { get; set; } = "ControllerServices";
    }
}
