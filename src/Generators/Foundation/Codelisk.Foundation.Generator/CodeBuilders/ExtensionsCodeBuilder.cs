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
                var properties = dto.GetAllProperties(true);
                var dtoReturnName = dto.GetReturnTypeName();
                var entityName = dto.GetEntityName();
                extensionsClass
                    .AddMethod("ToEntity", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + dtoReturnName, dto.Name.GetParameterName())
                    .WithBody(x =>
                    {
                        properties.Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine(
                            $"var result = new {entityName}({dto.Name.GetParameterName()});"
                        );
                        x.AppendLine("return result;");
                    })
                    .WithReturnType(entityName);

                extensionsClass
                    .AddMethod("ToEntities", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter(
                        "this " + $"List<{dtoReturnName}>",
                        dto.Name.GetParameterName(true)
                    )
                    .WithBody(x =>
                    {
                        properties.Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine($"var result = new List<{entityName}>();");
                        x.ForEach("var dto", dto.Name.GetParameterName(true))
                            .WithBody(y =>
                            {
                                y.AppendLine($"result.Add(new {entityName}(dto));");
                            });
                        x.AppendLine("return result;");
                    })
                    .WithReturnTypeList(entityName);

                extensionsClass
                    .AddMethod("ToDto", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + entityName, "entity")
                    .WithBody(x =>
                    {
                        properties.Where(x => x.DeclaredAccessibility == Accessibility.Public);

                        x.AppendLine($"return entity as {dto.Name};");
                    })
                    .WithReturnType(dto.Name);

                extensionsClass
                    .AddMethod("ToDtos", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + $"List<{entityName}>", "entities")
                    .WithBody(x =>
                    {
                        properties.Where(x => x.DeclaredAccessibility == Accessibility.Public);

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
