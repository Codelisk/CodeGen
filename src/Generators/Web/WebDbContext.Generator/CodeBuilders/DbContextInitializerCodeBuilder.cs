using Generators.Base.CodeBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextInitializerCodeBuilder : BaseModuleInitializerBuilder
    {
        public DbContextInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "DbContext";
    }
}
