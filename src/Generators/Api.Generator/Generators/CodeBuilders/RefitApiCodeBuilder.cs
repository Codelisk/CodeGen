using Attributes.WebAttributes.Repository;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Api.Generator.Generators.CodeBuilders
{
    public class RefitApiCodeBuilder : BaseCodeBuilder
    {
        public RefitApiCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var baseApi = context.BaseApi();
            var dtos = context.Dtos();

            return new() { BuildApi(context, dtos.First(), baseApi) };
        }
        private CodeBuilder BuildApi(GeneratorExecutionContext context, INamedTypeSymbol dto, INamedTypeSymbol baseApi)
        {
            var codeBuilder = CreateBuilder();
            var c = codeBuilder.AddClass(dto.Name).WithAccessModifier(Accessibility.Public).OfType(TypeKind.Interface).Abstract(false)
                .SetBaseClass(baseApi);

            Method(context, c, dto);

            return codeBuilder;
        }
        private MethodBuilder Method(GeneratorExecutionContext context, ClassBuilder c, INamedTypeSymbol dto)
        {
            return c.AddMethod(context.AttributeUrl<GetAttribute>(dto), Accessibility.NotApplicable)
                .AddAttribute(context.AttributeUrl<GetAttribute>(dto).AttributeWithConstructor("Get"))
                .WithBody(x =>
                {
                    x.AppendLine("");
                });
        }
    }
}
