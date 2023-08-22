using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;

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
            WebApiGenerator.line = 2;
            var baseRepo = context.DefaultApiRepository();

            WebApiGenerator.line = 3;
            var result = new List<CodeBuilder>();

            WebApiGenerator.line = 4;
            foreach (var dto in context.Dtos())
            {
                WebApiGenerator.line = 5;
                var codeBuilder = CreateBuilder();
                var repoName = dto.RepoName();
                WebApiGenerator.line = 6;
                var repoClass = codeBuilder.AddClass(repoName).WithAccessModifier(Accessibility.Public).SetBaseClass($"{baseRepo.Construct(dto).Name}<{dto.ApiName()}>");

                WebApiGenerator.line = 7;
                var constructor = repoClass.AddConstructor()
                    .WithBaseCall(baseRepo.InstanceConstructors.First().Parameters);


                var attrs = new Dictionary<Type, string>
            {
                {typeof(GetAttribute), "Get" },
                {typeof(GetAllAttribute), "Get" },
                {typeof(SaveAttribute), "Post" },
                {typeof(DeleteAttribute), "Delete" }
            };
                WebApiGenerator.line = 8;
                foreach (var attr in attrs)
                {
                    var httpAttributeSymbol = context.GetClass(attr.Key, "Codelisk.GeneratorAttributes");
                    var methodBuilder = repoClass.AddMethod(httpAttributeSymbol.AttributeUrl(dto), Accessibility.Public)
                        .WithReturnTypeForHttpMethod(attr.Key, dto)
                        .AddParametersForHttpMethod(httpAttributeSymbol, dto);
                    WebApiGenerator.line = 9;

                    var baseRepoMethod = baseRepo.GetMethodsWithAttribute(httpAttributeSymbol.Name).First();
                    methodBuilder.WithBody((x) =>
                    {
                        x.AppendLine($"return {baseRepoMethod.Name}(() => _repositoryApi.{httpAttributeSymbol.AttributeUrl(dto)}({string.Join(",", methodBuilder.Parameters.Select(x => x.Name.GetParameterName()))}));");
                    });
                }
                WebApiGenerator.line = 10;

                codeBuilder.GenerateInterface<RegisterSingleton>(context);

                result.Add(codeBuilder);

                //context.AddSource(dto.Name.Substring(1, dto.Name.Length - 1) + "Repository", refitApiCodeBuilder.Build());
            }
            return result;
        }
    }
}