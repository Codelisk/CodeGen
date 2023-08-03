using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using WebAutoMapperProfile.Generator.CodeBuilders;

namespace WebAutoMapperProfile.Generator.Generators
{
    [Generator]
    public class DtoEntityProfileGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            var dtoEntityProfileBuilder = new DtoEntityProfileBuilder(context.Compilation.AssemblyName).Get(context);
            AddSource(context, "AutoMapper", dtoEntityProfileBuilder);
        }
    }
}
