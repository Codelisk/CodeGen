using Controller.Generator.Generators.CodeBuilders;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Generator.Generators
{
    public class MainControllerGenerator : BaseGenerator
    {  
        public override void Execute(GeneratorExecutionContext context)
        {
            Debugger.Launch();    
            var codeBuilder = new ControllerCodeBuilder().Get(context);
            var dbContextCodeBuilder = new DbContextCodeBuilder().Get(context);
            var repositoryBuilder = new RepositoryCodeBuilder().Get(context);
            var managerCodeBuilder = new ManagerCodeBuilder().Get(context);

            AddSource(context, "DbContexts", dbContextCodeBuilder);
            AddSource(context, "Repositories", repositoryBuilder);
            AddSource(context, "Manager", managerCodeBuilder);

            var moduleInitializerBuilder = new ControllerModuleInitializerBuilder(context.Compilation.AssemblyName).Get(context, codeBuilder.Concat(managerCodeBuilder).Concat(repositoryBuilder).ToList());

            AddSource(context, "Controller", codeBuilder);
            AddSource(context, "", moduleInitializerBuilder);
        }
    }  
} 
