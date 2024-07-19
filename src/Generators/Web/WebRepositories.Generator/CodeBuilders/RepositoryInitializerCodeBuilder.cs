using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public RepositoryInitializerCodeBuilder(
            AttributeCompilationCrawler attributeCompilationCrawler
        )
            : base(
                attributeCompilationCrawler.GetInitNamespace<RepositoryModuleInitializerAttribute>(),
                attributeCompilationCrawler.GetInitMethodeName<RepositoryModuleInitializerAttribute>()
            ) { }
    }
}
