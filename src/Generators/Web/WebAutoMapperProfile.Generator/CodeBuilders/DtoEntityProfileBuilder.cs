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

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var dtos = context.Dtos().ToList();
            var result = CreateBuilder();
            result.AddClass(Constants.ProfileName).WithAccessModifier(Accessibility.Public).SetBaseClass(Constants.AutoMapperProfileBaseClass)
                .AddNamespaceImport(Constants.AutoMapperNamespaceImport)
                .AddDtoUsing(context)
                .AddEntityUsing(context)
                .AddConstructor()
                .WithBody(x =>
                {
                    foreach (var d in dtos)
                    {
                        x.AppendLine($"{Constants.AutoMapperCreateMap}<{d.Name}, {d.EntityFromDto(context).Name}>().{Constants.AutoMapperReverseMap}();");
                    }
                });

            return new List<CodeBuilder> { result };
        }
    }
}
