using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Codelisk.Foundation.Generator.CodeBuilders
{
    internal class EntityCodeBuilder : BaseCodeBuilder
    {
        public EntityCodeBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }

        public override List<CodeBuilder> Get(
            Compilation compilation,
            List<CodeBuilder> codeBuilders = null
        )
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(context);
            var dtos = attributeCompilationCrawler.Dtos().ToList();
            return Build(attributeCompilationCrawler, dtos);
        }

        private List<CodeBuilder?> Build(
            AttributeCompilationCrawler context,
            IEnumerable<INamedTypeSymbol> dtos
        )
        {
            var result = new List<CodeBuilder?>();

            foreach (var dto in dtos)
            {
                var builder = CreateBuilder(dtos.First().ContainingNamespace.ToString());
                Class(builder, dto, context);
                result.Add(builder);
            }

            return result;
        }

        private IReadOnlyList<ClassBuilder> Class(
            CodeBuilder builder,
            INamedTypeSymbol dto,
            AttributeCompilationCrawler context
        )
        {
            var result = builder
                .TopLevelNamespace()
                .AddClass(dto.GetEntityName())
                .WithAccessModifier(Accessibility.Public);

            result
                .AddProperty(dto.Name.GetParameterName(), Accessibility.Public)
                .SetType(dto.Name)
                .UseAutoProps();
            var dtoProperties = dto.GetAllProperties();

            foreach (var dtoProperty in dtoProperties)
            {
                result
                    .AddProperty(dtoProperty.Name, dtoProperty.DeclaredAccessibility)
                    .SetType(dtoProperty.ContainingType);
            }

            return builder.Classes;
        }
    }
}
