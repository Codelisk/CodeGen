using Attributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators.CodeBuilders
{
    public class RepositoryCodeBuilder : BaseControllerCodeBuilder
    {
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
                var builder = CreateBuilder();
                var baseRepository = context.Repository(dto);
                Class(builder, dto, null, baseRepository, context);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, ClassWithMethods repoModel, INamedTypeSymbol baseRepository, GeneratorExecutionContext context)
        {
            var constructedBaseRepo = baseRepository.ConstructFromDto(dto, context);
            return builder.AddClass(dto.RepositoryNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseRepo.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(nameof(GeneratedRepositoryAttribute))
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseRepo, dto.DbContextNameFromDto())
                .Class;
        }

    }
}