using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Generator.Generators.CodeBuilders.FromController
{
    public abstract class BaseWebApiCodeBuilder : BaseCodeBuilder
    {
        protected BaseWebApiCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }
        protected IEnumerable<INamedTypeSymbol> GetControllers(GeneratorExecutionContext context)
        {
            return context.GetClassesWithBaseClass(context.DefaultController());
        }
    }
}
