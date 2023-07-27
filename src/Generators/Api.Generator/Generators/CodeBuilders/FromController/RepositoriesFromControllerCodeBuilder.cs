using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Generator.Generators.CodeBuilders.FromController
{
    public class RepositoriesFromControllerCodeBuilder : BaseWebApiCodeBuilder
    {
        public RepositoriesFromControllerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            return GenerateRepositories(context, codeBuilders);
        }
        public List<CodeBuilder> GenerateRepositories(GeneratorExecutionContext context, List<CodeBuilder> refitApiCodeBuilder)
        {
            List<INamedTypeSymbol> allClasses = new List<INamedTypeSymbol>();
            foreach (var item in refitApiCodeBuilder)
            {
                allClasses.AddRange(item.GetClasses());
            }

            var result = new List<CodeBuilder>();
            //var baseApi = context.GetAllClasses("").Single(x => x.Name.Equals("IBaseApi"));
            foreach (var c in allClasses.Where(x => x.Interfaces.Any(x => SymbolEqualityComparer.Default.Equals(x, context.BaseApi()))).ToList())
            {
                var codeBuilder = CreateBuilder();
                var repoName = c.RepositoryNameFromApi();
                var repoClass = codeBuilder.AddClass(repoName).SetBaseClass(context.BaseRepository().Construct(c));

                var constructor = repoClass.AddConstructor()
                    .WithBaseCall(new Dictionary<string, string> { { "IBaseRepositoryProvider", "baseRepositoryProvider" } });

                foreach (var method in c.GetMethods())
                {
                    var methodBuilder = repoClass.AddMethod(method.Name, Accessibility.Public)
                        .WithReturnType(method.ReturnType.Name);

                    foreach (var parameter in method.Parameters)
                    {
                        methodBuilder.AddParameter(parameter.Type.GetReturnTypeName(), parameter.Name);
                    }

                    methodBuilder.WithBody((x) =>
                    {
                        x.AppendLine($"return TryRequest(() => _repositoryApi.{method.Name}({string.Join(",", method.Parameters.Select(x => x.Name))}));");
                    });
                }
                result.Add(codeBuilder);

                //context.AddSource(c.Name.Substring(1, c.Name.Length - 1) + "Repository", refitApiCodeBuilder.Build());
            }
            return result;
        }
    }
}
