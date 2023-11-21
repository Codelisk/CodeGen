using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.CodeBuilders
{
    public abstract class ServicesModuleInitializerBuilder : BaseOverrideModuleInitializerBuilder
    {
        public ServicesModuleInitializerBuilder(string codeBuilderNamespace, string overrideMethodeName) : base(codeBuilderNamespace, overrideMethodeName)
        {
        }

    }
}
