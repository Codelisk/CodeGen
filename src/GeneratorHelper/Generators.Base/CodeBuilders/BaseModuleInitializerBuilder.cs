using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Microsoft.CodeAnalysis;

namespace Generators.Base.CodeBuilders
{
    public abstract class BaseModuleInitializerBuilder : BaseCodeBuilder
    {
        public BaseModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }
        public abstract string ModuleName { get; set; }
        public virtual List<(string serviceUsage, string serviceType, string serviceImplementation)> Services { get; set; } = new List<(string serviceUsage, string serviceType, string serviceImplementation)>();
        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var builder = CreateBuilder();
            builder.AddClass("ModuleInitializerHelper").AddNamespaceImport("Microsoft.Extensions.DependencyInjection").WithAccessModifier(Accessibility.Public).MakeStaticClass()
                .AddMethod("Add" + ModuleName).WithAccessModifier(Accessibility.Public).MakeStaticMethod()
                .AddParameter("this IServiceCollection", "services")
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
