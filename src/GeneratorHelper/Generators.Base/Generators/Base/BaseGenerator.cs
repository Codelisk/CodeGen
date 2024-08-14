using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseGenerator : IIncrementalGenerator
    {
        public abstract void Initialize(IncrementalGeneratorInitializationContext context);

        protected void AddSource(
            IncrementalGeneratorInitializationContext context,
            IncrementalValueProvider<
                List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>
            > codeBuildersProvider
        )
        {
            context.RegisterImplementationSourceOutput(
                codeBuildersProvider,
                static (sourceProductionContext, codeBuildersTuples) =>
                {
                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }
    }
}
