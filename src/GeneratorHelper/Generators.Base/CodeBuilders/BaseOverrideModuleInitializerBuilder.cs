using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.CodeBuilders
{
    public abstract class BaseOverrideModuleInitializerBuilder : BaseModuleInitializerBuilder
    {
        private readonly string _overrideMethodeName;

        public BaseOverrideModuleInitializerBuilder(string codeBuilderNamespace, string overrideMethodeName) : base(codeBuilderNamespace)
        {
            _overrideMethodeName = overrideMethodeName;
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

            var result = base.Get(context);


            var builder = CreateBuilder();
            builder.AddClass("ModuleInitializer").WithAccessModifier(Accessibility.Internal).AddMethod(_overrideMethodeName, Accessibility.Public).Override().AddParameter("Microsoft.Extensions.DependencyInjection.IServiceCollection", "services").WithBody(x =>
            {
                x.AppendLine($"base.{_overrideMethodeName}(services);");
                x.AppendLine($"services.Add{ModuleName}();");
            });

            result.Add(builder);

            return result;
        }
    }
}
