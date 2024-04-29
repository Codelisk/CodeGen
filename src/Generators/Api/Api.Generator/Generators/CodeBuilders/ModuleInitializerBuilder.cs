using Generators.Base.CodeBuilders;

namespace Api.Generator.Generators.CodeBuilders
{
    public class ModuleInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ModuleInitializerBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }
    }
}
