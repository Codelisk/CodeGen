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
    [Generator]
    public class MainControllerGenerator : BaseGenerator
    { 
        public override void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch(); 
            var codeBuilder = new ControllerCodeBuilder().Get(context);
               
            var moduleInitializerBuilder = new ControllerModuleInitializerBuilder(context.Compilation.AssemblyName).Get(context, codeBuilder);
            codeBuilder.AddRange(moduleInitializerBuilder);  
                                              
            AddSource(context, "Controller", codeBuilder);
        }
    }  
} 
