using Generators.Base.CodeBuilders;

namespace Api.Generator.Generators.CodeBuilders
{
    public class ModuleInitializerBuilder : ServicesModuleInitializerBuilder
    {
        public ModuleInitializerBuilder(string codeBuilderNamespace, string overrideMethodeName) : base(codeBuilderNamespace, overrideMethodeName)
        {
        }

        public override string ModuleName { get; set; } = "Repositories";
    }
}
