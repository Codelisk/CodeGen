using Attributes.WebAttributes.Repository.Base;
using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

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
            var baseRepo = context.BaseRepository();
            var result = new List<CodeBuilder>();
            //var baseApi = context.GetAllClasses("").Single(x => x.Name.Equals("IBaseApi"));
            foreach (var dto in context.Dtos())
            {
                var codeBuilder = CreateBuilder();
                var repoName = dto.RepositoryNameFromApi();
                var repoClass = codeBuilder.AddClass(repoName).SetBaseClass(baseRepo.Construct(dto));

                var constructor = repoClass.AddConstructor()
                    .WithBaseCall(baseRepo.InstanceConstructors.First().Parameters);

                var httpAttributes = context.GetClassesWithAttribute(nameof(UrlAttribute));
                foreach (var httpAttribute in httpAttributes)
                {
                    var methodBuilder = repoClass.AddMethod(httpAttribute.AttributeUrl(dto), Accessibility.Public)
                        .WithReturnTypeForHttpMethod(httpAttribute, dto)
                        .AddParametersForHttpMethod(httpAttribute, dto);

                    var baseRepoMethod = baseRepo.GetMethodsWithAttribute(httpAttribute.Name).First();
                    methodBuilder.WithBody((x) =>
                    {
                        x.AppendLine($"return {baseRepoMethod.Name}(() => _repositoryApi.{httpAttribute.AttributeUrl(dto)}({string.Join(",", methodBuilder.Parameters.Select(x => x.Name.GetParameterName()))}));");
                    });
                }
                result.Add(codeBuilder);

                //context.AddSource(dto.Name.Substring(1, dto.Name.Length - 1) + "Repository", refitApiCodeBuilder.Build());
            }
            return result;
        }
    }
}