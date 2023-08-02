using Attributes.GeneralAttributes.Registration;
using Attributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Models;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using System;
using WebGenerator.Base;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryCodeBuilder : BaseCodeBuilder
    {
        public RepositoryCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
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
                var builder = CreateBuilder();
                var baseRepository = context.Repository(dto);
                Class(builder, dto, baseRepository, context);
                result.Add(builder);
            }

            return result;
        }
        private IReadOnlyList<ClassBuilder> Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol baseRepository, GeneratorExecutionContext context)
        {
            var constructedBaseRepo = baseRepository.ConstructFromDto(dto, context);
            var result = builder.AddClass(dto.RepositoryNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseRepo.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(nameof(GeneratedRepositoryAttribute))
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseRepo)
                .Class;


            return builder.GenerateInterface<RegisterTransient>(context).Classes;
        }

    }
}