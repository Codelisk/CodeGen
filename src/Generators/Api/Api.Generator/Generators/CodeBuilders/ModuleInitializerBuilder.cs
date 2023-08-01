using Generators.Base.CodeBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Generator.Generators.CodeBuilders
{
    public class ModuleInitializerBuilder : ServicesModuleInitializerBuilder
    {
        public ModuleInitializerBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "AddRepositories";
    }
}
