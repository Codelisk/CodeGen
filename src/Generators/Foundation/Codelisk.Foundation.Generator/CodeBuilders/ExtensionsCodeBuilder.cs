using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
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
                //Removed for performance.AddDtoUsing(context)
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
                            $"var result = new {dto.GetEntityName()}({dto.Name.GetParameterName()});"
                        );
                        x.AppendLine("return result;");
                    })
                    .WithReturnType(dto.GetEntityName());

                extensionsClass
                    .AddMethod("ToEntities", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter(
                        "this " + $"List<{dto.GetReturnTypeName()}>",
                        dto.Name.GetParameterName(true)
                    )
                    .WithBody(x =>
                    {
                        var properties = dto.GetAllProperties(true)
                            .Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine($"var result = new List<{dto.GetEntityName()}>();");
                        x.ForEach("var dto", dto.Name.GetParameterName(true))
                            .WithBody(y =>
                            {
                                y.AppendLine($"result.Add(new {dto.GetEntityName()}(dto));");
                            });
                        x.AppendLine("return result;");
                    })
                    .WithReturnTypeList(dto.GetEntityName());

                extensionsClass
                    .AddMethod("ToDto", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + dto.GetEntityName(), "entity")
                    .WithBody(x =>
                    {
                        var properties = dto.GetAllProperties(true)
                            .Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine($"return entity as {dto.Name};");
                    })
                    .WithReturnType(dto.Name);

                extensionsClass
                    .AddMethod("ToDtos", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + $"List<{dto.GetEntityName()}>", "entities")
                    .WithBody(x =>
                    {
                        var properties = dto.GetAllProperties(true)
                            .Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine($"var result = new List<{dto.Name}>();");
                        x.ForEach("var entity", "entities")
                            .WithBody(y =>
                            {
                                y.AppendLine($"result.Add(entity as {dto.Name});");
                            });
                        x.AppendLine("return result;");
                    })
                    .WithReturnTypeList(dto.Name);
            }

            result.Add(builder);
            return result;
        }
    }
}
