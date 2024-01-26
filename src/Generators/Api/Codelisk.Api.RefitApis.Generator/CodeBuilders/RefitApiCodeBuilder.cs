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
using Codelisk.GeneratorAttributes.Helper;

namespace Api.RefitApis.Generator.CodeBuilders
{
    public class RefitApiCodeBuilder : BaseCodeBuilder
    {
        public RefitApiCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(Compilation context, List<CodeBuilder> codeBuilders = null)
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(context);
            var baseApi = attributeCompilationCrawler.BaseApi();
            var dtos = attributeCompilationCrawler.Dtos();
            List<CodeBuilder> result = new();
            foreach (var d in dtos)
            {
                result.Add(BuildApi(attributeCompilationCrawler, context, d, baseApi));
            }

            return result;
        }
        private CodeBuilder BuildApi(AttributeCompilationCrawler attributeCompilationCrawler, Compilation compilation, INamedTypeSymbol dto, INamedTypeSymbol baseApi)
        {
            var codeBuilder = CreateBuilder();
            var c = codeBuilder.AddClass(dto.ApiName()).WithAccessModifier(Accessibility.Public).OfType(TypeKind.Interface).Abstract(false)
                .AddNamespaceImport(Constants.RefitNamespaceImport)
                .SetBaseClass(baseApi);

            Method(attributeCompilationCrawler, compilation, c, dto);

            return codeBuilder;
        }
        private ClassBuilder Method(AttributeCompilationCrawler attributeCompilationCrawler, Compilation compilation, ClassBuilder c, INamedTypeSymbol dto)
        {
            var typeAndRefitAttribute = AttributeHelper.AllAttributesMethodeHeaderDictionary();

            foreach (var attr in typeAndRefitAttribute)
            {
                var attributeUrl = attributeCompilationCrawler.AttributeUrl(attr.Key, dto);
                var httpAttributeSymbol = compilation.GetClass(attr.Key, "Codelisk.GeneratorAttributes");
                c.AddMethod(attributeUrl, Accessibility.NotApplicable).Abstract(true)
                    .AddAttribute($"/{dto.ReplaceDtoSuffix()}/{attributeUrl}".AttributeWithConstructor($"{attr.Value}"))
                    .AddParametersForHttpMethod(httpAttributeSymbol, dto)
                    .WithReturnTypeForHttpMethod(attr.Key, dto);
            }
            return c;
        }
    }
}
