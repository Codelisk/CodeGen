﻿using Attributes.GeneratorAttributes;
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
    public class DbContextCodeBuilder : BaseControllerCodeBuilder
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
                var baseContext = context.BaseContext();
                Class(builder, dto, null, baseContext, context);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, ClassWithMethods repoModel, INamedTypeSymbol baseRepository, GeneratorExecutionContext context)
        {
            var constructedBaseRepo = baseRepository.ConstructFromDto(dto, context);
            return builder.AddClass(dto.DbContextNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseRepo.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(nameof(GeneratedDbContextAttribute))
                .AddConstructor()
                .BaseConstructorTypeParameterParameterBaseCall(constructedBaseRepo)
                .Class;
        }

    }
}