using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public ManagerInitializerCodeBuilder(
            (string codeBuilderNamespace, string addServicesMethodeName) nameSpaceMethode
        )
            : base(nameSpaceMethode.codeBuilderNamespace, nameSpaceMethode.addServicesMethodeName)
        { }
    }
}
