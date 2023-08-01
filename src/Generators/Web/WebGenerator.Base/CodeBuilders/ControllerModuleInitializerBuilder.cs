using CodeGenHelpers;
using Generators.Base.CodeBuilders;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebGenerator.Base.CodeBuilders
{
    public class ControllerModuleInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ControllerModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        { 
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            Services = new List<(string, string, string)>();
            //AddDbContexts(context);

            return base.Get(context, codeBuilders);
        }

        //private void AddDbContexts(GeneratorExecutionContext context)
        //{
        //    var baseContext = context.BaseContext();
        //    var dtos = context.Dtos();
        //    foreach (var dto in dtos)
        //    {
        //        Services.Add(("AddDbContext", baseContext.Construct(dto).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat), string.Empty));
        //    }
        //}

        public override string ModuleName { get; set; } = "ControllerServices";
    }
}
