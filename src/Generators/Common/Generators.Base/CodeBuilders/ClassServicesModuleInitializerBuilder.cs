using Attributes.GeneralAttributes.Registration;
using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
            if (Services is null)
            {
                Services = new List<(string, string, string)>();
            }

            List<string> serviceTypes=new List<string>();
            if(codeBuilders is not null)
            {
                foreach (var codeBuilder in codeBuilders)
                {
                    foreach (var c in codeBuilder.GetClasses(context))
                    {
                        var test = c.GetAttributes();
                        var registerAttribute = c.GetAttributes().FirstOrDefault(x => nameof(RegisterTransient).Equals(x.AttributeClass.Name));

                        if (registerAttribute is not null)
                        {
                            var type = registerAttribute.GetFirstConstructorArgumentAsTypedConstant().Value as Type;
                            if (type is not null)
                            {
                                Services.Add((null, type.Name, c.Name));
                            }
                            else
                            {
                                Services.Add((null, c.Interfaces.FirstOrDefault()?.Name, c.Name));
                            }
                        }
                    }
                }
            }

            foreach (var serviceType in serviceTypes)
            {
                Services.Add(("AddTransient", serviceType, ""));
            }

            return base.Get(context);
        }
    }
}
