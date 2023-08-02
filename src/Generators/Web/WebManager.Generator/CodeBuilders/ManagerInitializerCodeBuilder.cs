using CodeGenHelpers;
using Generator.Foundation.Generators.Base;
using Generators.Base.CodeBuilders;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerInitializerCodeBuilder : BaseModuleInitializerBuilder
    {
        public ManagerInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "Manager";
    }
}
