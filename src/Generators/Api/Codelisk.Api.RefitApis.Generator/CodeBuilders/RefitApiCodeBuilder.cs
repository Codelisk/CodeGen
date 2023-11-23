using CodeGenHelpers;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using Foundation.Crawler.Extensions.Extensions;
using System.Text;

namespace Api.RefitApis.Generator.CodeBuilders
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
            List<CodeBuilder> result = new();
            foreach (var d in dtos)
            {
                result.Add(BuildApi(context, d, baseApi));
            }

            return result;
        }
        private CodeBuilder BuildApi(GeneratorExecutionContext context, INamedTypeSymbol dto, INamedTypeSymbol baseApi)
        {
            var codeBuilder = CreateBuilder();
            var c = codeBuilder.AddClass(dto.ApiName()).WithAccessModifier(Accessibility.Public).OfType(TypeKind.Interface).Abstract(false)
                .AddNamespaceImport(Constants.RefitNamespaceImport)
                .SetBaseClass(baseApi);

            Method(context, c, dto);

            return codeBuilder;
        }
        private ClassBuilder Method(GeneratorExecutionContext context, ClassBuilder c, INamedTypeSymbol dto)
        {
            var typeAndRefitAttribute = new Dictionary<Type, string>
            {
                {typeof(GetAttribute), "Get" },
                {typeof(GetFullAttribute), "Get" },
                {typeof(GetAllAttribute), "Get" },
                {typeof(GetAllFullAttribute), "Get" },
                {typeof(SaveAttribute), "Post" },
                {typeof(DeleteAttribute), "Delete" }
            };

            foreach (var attr in typeAndRefitAttribute)
            {
                var attributeUrl = context.AttributeUrl(attr.Key, dto);
                var httpAttributeSymbol = context.GetClass(attr.Key, "Codelisk.GeneratorAttributes");
                c.AddMethod(attributeUrl, Accessibility.NotApplicable).Abstract(true)
                    .AddAttribute($"/{dto.ReplaceDtoSuffix()}/{attributeUrl}".AttributeWithConstructor($"{attr.Value}"))
                    .AddParametersForHttpMethod(httpAttributeSymbol, dto)
                    .WithReturnTypeForHttpMethod(attr.Key, dto);
            }
            return c;
        }
    }
}
