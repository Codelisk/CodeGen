using Generators.Base.CodeBuilders;

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
