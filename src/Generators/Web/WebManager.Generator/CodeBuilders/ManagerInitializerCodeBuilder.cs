using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public ManagerInitializerCodeBuilder(
            AttributeCompilationCrawler attributeCompilationCrawler
        )
            : base(
                attributeCompilationCrawler.GetInitNamespace<ManagerModuleInitializerAttribute>(),
                attributeCompilationCrawler.GetInitMethodeName<ManagerModuleInitializerAttribute>()
            ) { }
    }
}
