using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;

namespace WebAutoMapperProfile.Generator.CodeBuilders
{
    public class DtoEntityProfileBuilder : BaseCodeBuilder
    {
        public DtoEntityProfileBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(Compilation context, List<CodeBuilder> codeBuilders = null)
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(context);
            var dtos = attributeCompilationCrawler.Dtos().ToList();
            var result = CreateBuilder();
            result.AddClass(Constants.ProfileName).WithAccessModifier(Accessibility.Public).SetBaseClass(Constants.AutoMapperProfileBaseClass)
                .AddNamespaceImport(Constants.AutoMapperNamespaceImport)
                .AddDtoUsing(attributeCompilationCrawler)
                .AddEntityUsing(attributeCompilationCrawler, context)
                .AddConstructor()
                .WithBody(x =>
                {
                    foreach (var d in dtos)
                    {
                        x.AppendLine($"{Constants.AutoMapperCreateMap}<{d.Name}, {d.EntityFromDto(context).Name}>().{Constants.AutoMapperReverseMap}();");
                        //x.AppendLine($"{Constants.AutoMapperCreateMap}<List<{d.Name}>, List<{d.EntityFromDto(context).Name}>>().{Constants.AutoMapperReverseMap}();");
                    }
                });

            return new List<CodeBuilder> { result };
        }
    }
}
