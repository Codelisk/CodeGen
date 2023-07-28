using CodeGenHelpers;
using Generators.Base.Extensions;
using Generators.Base.Generators.Base;
using Maui.Generator.Extensions;
using Maui.Generator.Extensions.Xaml;
using Maui.Generator.Generators.Codebuilder;
using Maui.Generator.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Maui.Generator.Generators
{
    [Generator]
    public class BaseViewModelGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {   
            //Debugger.Launch();
            var viewModels = new BaseViewModelBuilder(context.Compilation.AssemblyName).Get(context);
             
            AddSource(context,"ViewModels",viewModels);
        }

    }
}
