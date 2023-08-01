using CodeGenHelpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Foundation.Generators.Base
{
    public abstract class BaseCodeBuilder
    {
        private readonly string CodeBuilderNamespace;
        public BaseCodeBuilder(string codeBuilderNamespace)
        {
            this.CodeBuilderNamespace = codeBuilderNamespace;
        }
        public abstract List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null);
        public CodeBuilder CreateBuilder()
        {
            return CodeBuilder.Create(CodeBuilderNamespace);
        }
    }
}
