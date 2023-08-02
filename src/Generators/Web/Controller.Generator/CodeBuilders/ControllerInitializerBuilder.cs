using CodeGenHelpers;
using Generators.Base.CodeBuilders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.CodeBuilders
{
    public class ControllerInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ControllerInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "ControllerServices";
    }
}
