﻿
using Controller.Generator.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators
{
    [Generator]
    public class ControllerGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var codeBuilder = new ControllerCodeBuilder(context.Compilation.AssemblyName).Get(context);
            var initializerBuilder = new ControllerInitializerBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "Controller", codeBuilder);
            AddSource(context, "", initializerBuilder);
        }
    }
}