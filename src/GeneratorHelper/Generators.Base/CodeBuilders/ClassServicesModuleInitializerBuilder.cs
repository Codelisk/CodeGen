using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

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

            List<string> serviceTypes = new List<string>();
            if (codeBuilders is not null)
            {
                foreach (var codeBuilder in codeBuilders)
                {
                    foreach (var c in codeBuilder.GetClasses(context))
                    {
                        var registerAttribute = c.GetAttributes().FirstOrDefault(x => nameof(RegisterTransient).Equals(x.AttributeClass.Name));

                        if (registerAttribute is not null)
                        {
                            var type = registerAttribute.GetFirstConstructorArgumentAsTypedConstant().Value as Type;
                            if (type is not null)
                            {
                                Services.Add(("AddTransient", type.Name, c.Name));
                            }
                            else
                            {
                                Services.Add(("AddTransient", c.Interfaces.FirstOrDefault()?.Name, c.Name));
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
        private void Add<TAttribute>(INamedTypeSymbol c) where TAttribute : BaseRegisterAttribute
        {
            var attribute = c.GetAttributes().FirstOrDefault(x => typeof(TAttribute).Name.Equals(x.AttributeClass.Name));
            string implementationType = "AddSingleton";
            if (attribute is RegisterTransient)
            {
                implementationType = "AddTransient";
            }
            else if (attribute is RegisterSingleton)
            {

            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            var type = attribute.GetFirstConstructorArgumentAsTypedConstant().Value as Type;
            if (type is not null)
            {
                Services.Add((implementationType, type.Name, c.Name));
            }
            else
            {
                Services.Add((implementationType, c.Interfaces.FirstOrDefault()?.Name, c.Name));
            }
        }
    }
}
