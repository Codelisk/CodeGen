using Generators.Base.CodeBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public RepositoryInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "Repositories";
    }
}
