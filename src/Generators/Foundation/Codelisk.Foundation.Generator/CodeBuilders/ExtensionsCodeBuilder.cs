using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Codelisk.Foundation.Generator.CodeBuilders
{
    internal class ExtensionsCodeBuilder : BaseCodeBuilder
    {
        public ExtensionsCodeBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }

        public override List<CodeBuilder> Get(
            Compilation compilation,
            List<CodeBuilder> codeBuilders = null
        )
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(compilation);
            var dtos = attributeCompilationCrawler.Dtos().ToList();
            return Build(attributeCompilationCrawler, dtos);
        }

        private List<CodeBuilder?> Build(
            AttributeCompilationCrawler context,
            IEnumerable<INamedTypeSymbol> dtos
        )
        {
            var result = new List<CodeBuilder?>();

            var builder = CreateBuilder(dtos.First().ContainingNamespace.ToString());

            var extensionsClass = builder
                .AddClass("DtoEntityExtensions")
                .MakeStaticClass()
                .MakePublicClass();

            foreach (var dto in dtos)
            {
                extensionsClass
                    .AddMethod("ToEntity", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + dto.GetReturnTypeName(), dto.Name.GetParameterName())
                    .WithBody(x =>
                    {
                        var properties = dto.GetAllProperties(true)
                            .Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine(
                            $"var result = new {dto.GetEntityName()}({dto.Name.GetParameterName()})"
                        );
                        x.AppendLine("return result");
                    })
                    .WithReturnType(dto.GetEntityName());
            }

            result.Add(builder);
            return result;
        }
    }
}
