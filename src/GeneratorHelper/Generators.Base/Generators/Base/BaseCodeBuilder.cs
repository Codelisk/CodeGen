﻿using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace Generator.Foundation.Generators.Base
{
    public abstract class BaseCodeBuilder
    {
        public readonly string CodeBuilderNamespace;

        public BaseCodeBuilder(string codeBuilderNamespace)
        {
            CodeBuilderNamespace = codeBuilderNamespace;
        }

        public abstract List<CodeBuilder> Get(
            Compilation compilation,
            List<CodeBuilder> codeBuilders = null
        );

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
