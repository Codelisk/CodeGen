using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseModuleInitializerGenerator : BaseGenerator
    {
        public static List<CodeBuilder> Get(
            CodeBuilder builder,
            List<(string serviceUsage, string serviceType, string serviceImplementation)> services,
            string methodeName
        )
        {
            builder
                .AddClass("ModuleInitializer")
                .AddNamespaceImport("Microsoft.Extensions.DependencyInjection")
                .WithAccessModifier(Accessibility.Public)
                .AddMethod($"partial {methodeName}", Accessibility.NotApplicable)
                .AddParameter("IServiceCollection", "services")
                .WithBody(x =>
                {
                    foreach (var service in services)
                    {
                        var serviceUsage = string.IsNullOrEmpty(service.serviceUsage)
                            ? "AddScoped"
                            : service.serviceUsage;
                        var serviceImplementation = service.serviceImplementation is null
                            ? string.Empty
                            : service.serviceImplementation;

                        if (!string.IsNullOrEmpty(serviceImplementation))
                        {
                            x.AppendLine(
                                $"services.{serviceUsage}<{service.serviceType},{serviceImplementation}>();"
                            );
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
