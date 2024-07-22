﻿using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
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
                .SetBaseClass(dto)
                .AddAttribute($"{typeof(EntityAttribute).FullName}(typeof({dto.Name}))")
                .WithAccessModifier(Accessibility.Public);

            var constructor = result.AddConstructor().AddParameter(dto);

            var properties = dto.GetAllProperties(true)
                .Where(x => x.DeclaredAccessibility == Accessibility.Public);

            constructor.WithBody(x =>
            {
                foreach (var property in properties)
                {
                    x.AppendLine(
                        $"this.{property.Name} = {dto.Name.GetParameterName()}.{property.Name};"
                    );
                }
            });

            return builder.Classes;
        }
    }
}