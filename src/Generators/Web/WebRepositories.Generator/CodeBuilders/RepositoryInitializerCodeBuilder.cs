using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public RepositoryInitializerCodeBuilder(
            (string codeBuilderNamespace, string addServicesMethodeName) nameSpaceMethode
        )
            : base(nameSpaceMethode.codeBuilderNamespace, nameSpaceMethode.addServicesMethodeName)
        { }
    }
}
