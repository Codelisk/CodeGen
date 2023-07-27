using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Generator.Generators.CodeBuilders
{
    public class ControllerCodeBuilder : BaseControllerCodeBuilder
    {
        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var dtos = context.Dtos().ToList();
            var defaultRepository = context.DefaultRepository();
            var baseController = context.BaseController();
            return Build(dtos, defaultRepository, baseController);
        }

        private List<CodeBuilder?> Build(IEnumerable<INamedTypeSymbol> dtos, DefaultRepositoryModel repoModel, INamedTypeSymbol baseController)
        {
            var result = new List<CodeBuilder?>();
            foreach (var dto in dtos)
            {
                var builder = CreateBuilder();

                var c = Class(builder, dto, repoModel, baseController);

                Methods(c, dto, baseController, repoModel);

                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, DefaultRepositoryModel repoModel, INamedTypeSymbol baseController)
        {
            return builder.AddClass(dto.ControllerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(baseController.Construct(dto).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .WithBaseCall(new Dictionary<string, string> { { $"{repoModel.Repo.Name}<{dto.Name}>", repoModel.Repo.Name.GetParameterName() } })
                .Class;
        }
        private void Methods(ClassBuilder c, INamedTypeSymbol dto, INamedTypeSymbol baseController, DefaultRepositoryModel repoModel)
        {
            var repoProperty = baseController.GetFieldsWithConstructedFromType(repoModel.Repo).First();

            Dictionary<IMethodSymbol, string> properties = new Dictionary<IMethodSymbol, string>()
            {
                {repoModel.Delete, "HttpDelete" },
                {repoModel.Get, "HttpGet" },
                {repoModel.GetAll, "HttpGet" },
                {repoModel.Save, "HttpPost" },
            };

            foreach (var item in properties)
            {
                var methodBuilder = c.AddMethod(item.Key.MethodName(dto), Accessibility.Public)
                    .AddAttribute(item.Key.HttpControllerAttribute(dto, item.Value))
                    .AddParametersForHttpMethod(item.Key.HttpAttribute(), dto)
                    .WithReturnType(item.Key.ReturnType.Name);

                methodBuilder.WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{item.Key.MethodName(dto)}({methodBuilder.Parameters.Select(x=>x.Name.GetParameterName())});");
                });
            }
        }

    }
}
