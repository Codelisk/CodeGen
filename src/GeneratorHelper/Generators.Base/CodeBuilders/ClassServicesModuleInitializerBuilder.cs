using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Generator.Foundation.Generators.Base;

namespace Generators.Base.CodeBuilders
{
    public abstract class ClassServicesModuleInitializerBuilder : BaseCodeBuilder
    {
        public ClassServicesModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }
        public virtual List<(string serviceUsage, string serviceType, string serviceImplementation)> Services { get; set; } = new List<(string serviceUsage, string serviceType, string serviceImplementation)>();

        public override List<CodeBuilder> Get(Compilation compilation, List<CodeBuilder> codeBuilders = null)
        {
            var builder = CreateBuilder();
            builder.AddClass("ModuleInitializer").AddNamespaceImport("Microsoft.Extensions.DependencyInjection").WithAccessModifier(Accessibility.Public)
                .AddMethod("partial AddServices").Abstract(true)
                .AddParameter("IServiceCollection", "services")
                .WithBody(x =>
                {
                    foreach (var service in Services)
                    {
                        var serviceUsage = string.IsNullOrEmpty(service.serviceUsage) ? "AddTransient" : service.serviceUsage;
                        var serviceImplementation = service.serviceImplementation is null ? string.Empty : service.serviceImplementation;

                        if (!string.IsNullOrEmpty(serviceImplementation))
                        {
                            x.AppendLine($"services.{serviceUsage}<{service.serviceType},{serviceImplementation}>();");
                        }
                        else
                        {
                            x.AppendLine($"services.{serviceUsage}<{service.serviceType}>();");
                        }
                    }
                });

            return new List<CodeBuilder> { builder };
        }
    }
}
