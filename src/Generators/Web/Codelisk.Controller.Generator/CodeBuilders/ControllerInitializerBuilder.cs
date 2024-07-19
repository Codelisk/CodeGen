using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Foundation.Crawler.Crawlers;
using Generators.Base.CodeBuilders;

namespace Controller.Generator.CodeBuilders
{
    public class ControllerInitializerBuilder : ClassServicesModuleInitializerBuilder
    {
        public ControllerInitializerBuilder(AttributeCompilationCrawler attributeCompilationCrawler)
            : base(
                attributeCompilationCrawler.GetInitNamespace<ControllerModuleInitializerAttribute>(),
                attributeCompilationCrawler.GetInitMethodeName<ControllerModuleInitializerAttribute>()
            ) { }
    }
}
