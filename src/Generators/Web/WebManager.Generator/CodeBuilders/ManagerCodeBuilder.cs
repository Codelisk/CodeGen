using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerCodeBuilder : BaseCodeBuilder
    {
        public ManagerCodeBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }

        public override List<CodeBuilder> Get(
            Compilation compilation,
            List<CodeBuilder> codeBuilders = null
        )
        {
            return default;
            //var attributeCompilationCrawler = new AttributeCompilationCrawler(compilation);
            //var dtos = attributeCompilationCrawler.Dtos().ToList();
            //return Build(attributeCompilationCrawler, compilation, dtos);
        }
    }
}
