using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace Controller.Generator.CodeBuilders
{
    public class ControllerInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ControllerInitializerBuilder(
            (string codeBuilderNamespace, string addServicesMethodeName) nameSpaceMethode
        )
            : base(nameSpaceMethode.codeBuilderNamespace, nameSpaceMethode.addServicesMethodeName)
        { }
    }
}
