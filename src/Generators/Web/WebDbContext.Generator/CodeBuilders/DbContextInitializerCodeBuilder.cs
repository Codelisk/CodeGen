using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public DbContextInitializerCodeBuilder(
            (string codeBuilderNamespace, string addServicesMethodeName) nameSpaceMethode
        )
            : base(nameSpaceMethode.codeBuilderNamespace, nameSpaceMethode.addServicesMethodeName)
        { }
    }
}
