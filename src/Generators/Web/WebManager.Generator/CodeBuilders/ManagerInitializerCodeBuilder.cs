using Generators.Base.CodeBuilders;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public ManagerInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "Manager";
    }
}
