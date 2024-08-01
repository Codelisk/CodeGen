using System;
using System.Collections.Generic;
using System.Text;
using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseSyntaxCodeBuilder
    {
        public readonly string CodeBuilderNamespace;

        public BaseSyntaxCodeBuilder(string codeBuilderNamespace)
        {
            CodeBuilderNamespace = codeBuilderNamespace;
        }

        public abstract List<CodeBuilder> Get(IncrementalGeneratorInitializationContext context);

        public CodeBuilder CreateBuilder(string ns = null)
        {
            if (ns is null)
            {
                return CodeBuilder.Create(CodeBuilderNamespace);
            }
            else
            {
                return CodeBuilder.Create(ns);
            }
        }
    }
}
