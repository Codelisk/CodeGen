using Generators.Base.CodeBuilders;

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
