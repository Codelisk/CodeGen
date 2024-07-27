using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Codelisk.Foundation.Generator.CodeBuilders
{
    public class FullModelCodeBuilder : BaseCodeBuilder
    {
        public FullModelCodeBuilder(string codeBuilderNamespace)
            : base(codeBuilderNamespace) { }

        public override List<CodeBuilder> Get(
            Compilation context,
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
                .AddClass(dto.GetFullModelName())
                .WithAccessModifier(Accessibility.Public);

            result
                .AddProperty(dto.Name.GetParameterName(), Accessibility.Public)
                .SetType(dto.Name)
                .UseAutoProps();
            //Removed for performance result.AddDtoUsing(context);
            var dtoPropertiesWithForeignKey = dto.DtoForeignProperties();

            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetPropertyAttributeValue(
                    AttributeNames.ForeignKey
                );
                var foreignKeyDto = context.Dtos().First(x => x.Name == foreignKeyName);

                result
                    .AddProperty(dtoProperty.GetFullModelNameFromProperty(), Accessibility.Public)
                    .SetType(foreignKeyDto.GetFullModelName())
                    .UseAutoProps();
            }

            return builder.Classes;
        }
    }
}
