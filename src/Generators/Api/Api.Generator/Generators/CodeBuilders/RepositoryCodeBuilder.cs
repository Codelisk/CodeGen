using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Generators.Base;
using Codelisk.GeneratorAttributes.Helper;

namespace Api.Generator.Generators.CodeBuilders
{
    public class RepositoryCodeBuilder : BaseCodeBuilder
    {
        public RepositoryCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            return GenerateRepositories(context, codeBuilders);
        }
        public List<CodeBuilder> GenerateRepositories(GeneratorExecutionContext context, List<CodeBuilder> refitApiCodeBuilder)
        {
            var baseRepo = context.DefaultApiRepository();

            var result = new List<CodeBuilder>();

            foreach (var dto in context.Dtos())
            {
                var codeBuilder = CreateBuilder();
                var repoName = dto.RepoName();
                var repoClass = codeBuilder.AddClass(repoName).WithAccessModifier(Accessibility.Public).SetBaseClass($"{baseRepo.Construct(dto).Name}<{dto.ApiName()}>");

                var constructor = repoClass.AddConstructor()
                    .WithBaseCall(baseRepo.InstanceConstructors.First().Parameters);


                var attrs = AttributeHelper.AllAttributesMethodeHeaderDictionary();
                foreach (var attr in attrs)
                {
                    try
                    {

                    var httpAttributeSymbol = context.GetClass(attr.Key, "Codelisk.GeneratorAttributes");
                    var methodBuilder = repoClass.AddMethod(httpAttributeSymbol.AttributeUrl(dto), Accessibility.Public)
                        .WithReturnTypeForHttpMethod(attr.Key, dto)
                        .AddParametersForHttpMethod(httpAttributeSymbol, dto);

                    var baseRepoMethod = baseRepo.GetMethodsWithAttributesIncludingBaseTypes(httpAttributeSymbol.Name).First();
                    methodBuilder.WithBody((x) =>
                    {
                        x.AppendLine($"return {baseRepoMethod.Name}(() => _repositoryApi.{httpAttributeSymbol.AttributeUrl(dto)}({string.Join(",", methodBuilder.Parameters.Select(x => x.Name.GetParameterName()))}));");
                    });
                    }
                    catch(Exception ex) { }
                }

                TestLog.Add("Start");
                codeBuilder.GenerateInterface<RegisterSingleton>(context);

                result.Add(codeBuilder);

                //context.AddSource(dto.Name.Substring(1, dto.Name.Length - 1) + "Repository", refitApiCodeBuilder.Build());
            }
            return result;
        }
    }
}
