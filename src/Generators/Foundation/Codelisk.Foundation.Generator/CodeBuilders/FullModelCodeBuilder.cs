using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Shared.Constants;

namespace Codelisk.Foundation.Generator.CodeBuilders
{
    public class FullModelCodeBuilder : BaseCodeBuilder
    {
        public FullModelCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var dtos = context.Dtos().ToList();
            return Build(context, dtos);
        }
        private List<CodeBuilder?> Build(GeneratorExecutionContext context, IEnumerable<INamedTypeSymbol> dtos)
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
        private IReadOnlyList<ClassBuilder> Class(CodeBuilder builder, INamedTypeSymbol dto, GeneratorExecutionContext context)
        {
            var result = builder.AddClass(dto.GetFullModelName()).WithAccessModifier(Accessibility.Public);

            result.AddProperty(dto.Name.GetParameterName(), Accessibility.Public).SetType(dto.Name).UseAutoProps();
            var dtoPropertiesWithForeignKey = dto.DtoForeignProperties();

            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetPropertyAttributeValue(AttributeNames.ForeignKey);
                var foreignKeyDto = context.Dtos().First(x => x.Name == foreignKeyName);
                
                result.AddProperty(dtoProperty.GetFullModelNameFromProperty(), Accessibility.Public).SetType(foreignKeyDto.GetFullModelName()).UseAutoProps();
                
            }

            return builder.Classes;
        }
    }
}
