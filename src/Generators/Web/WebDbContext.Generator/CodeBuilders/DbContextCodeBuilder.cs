﻿using Attributes;
using Attributes.GeneralAttributes.Registration;
using Attributes.GeneratorAttributes;
using Attributes.WebAttributes.Database;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Models;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextCodeBuilder : BaseCodeBuilder
    {
        public DbContextCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
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
            foreach (var groupedDtos in dtos.GroupBy(x => x.GetAttribute<DtoAttribute>().GetFirstConstructorArgument()))
            {
                var builder = CreateBuilder();
                var baseContext = context.BaseContext();
                Class(builder, groupedDtos, baseContext, context);
                result.Add(builder);
            }

            return result;
        }
        private IReadOnlyList<ClassBuilder> Class(CodeBuilder builder, IGrouping<string, INamedTypeSymbol> dtos, INamedTypeSymbol baseContext, GeneratorExecutionContext context)
        {
            var result = builder.AddClass(dtos.Key).WithAccessModifier(Accessibility.Public)
                .AddAttribute(nameof(GeneratedDbContextAttribute))
                .AddConstructor()
                .WithBaseCall(baseContext.InstanceConstructors.First())
                .Class;

            foreach (var dto in dtos)
            {
                result.AddProperty(dto.Name.GetParameterName(), Accessibility.Public).SetType($"DbSet<{dto.Name}").UseAutoProps();
            }

            return builder.Classes;
        }

    }
}