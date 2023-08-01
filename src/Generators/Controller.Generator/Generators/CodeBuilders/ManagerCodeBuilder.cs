using Attributes.GeneratorAttributes;
using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Repository;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Generator.Generators.CodeBuilders
{
    public class ManagerCodeBuilder : BaseControllerCodeBuilder
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
                var baseManager = context.Manager(dto);
                Class(builder, dto, context.Repository(dto), baseManager, context);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol baseRepo, INamedTypeSymbol baseManager, GeneratorExecutionContext context)
        {
            var constructedBaseManager = baseManager.ConstructFromDto(dto, context);
            return builder.AddClass(dto.ManagerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseManager.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(nameof(GeneratedManagerAttribute))
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseManager, (baseRepo, dto.RepositoryNameFromDto()))
                .Class;
        }

    }
}
