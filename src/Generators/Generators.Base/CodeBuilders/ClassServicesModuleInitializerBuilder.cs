using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.CodeBuilders
{
    public abstract class ClassServicesModuleInitializerBuilder : BaseModuleInitializerBuilder
    {
        public ClassServicesModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            List<string> serviceTypes=new List<string>();
            foreach (var codeBuilder in codeBuilders)
            {
                foreach (var c in codeBuilder.Classes)
                {
                    var parameters = c.Constructors.First().Parameters;
                    foreach (var parameter in parameters)
                    {
                        serviceTypes.Add(parameter.Type);
                    }
                }
            }

            if(Services is null)
            {
                Services = new List<(string, string, string)>();
            }

            foreach (var serviceType in serviceTypes)
            {
                Services.Add(("AddTransient", serviceType, ""));
            }

            return base.Get(context);
        }
    }
}
