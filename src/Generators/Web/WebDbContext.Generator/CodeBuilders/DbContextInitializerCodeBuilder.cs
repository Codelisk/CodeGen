using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public DbContextInitializerCodeBuilder(
            AttributeCompilationCrawler attributeCompilationCrawler
        )
            : base(
                attributeCompilationCrawler.GetInitNamespace<DatabaseModuleInitializerAttribute>(),
                attributeCompilationCrawler.GetInitMethodeName<DatabaseModuleInitializerAttribute>()
            ) { }
    }
}
