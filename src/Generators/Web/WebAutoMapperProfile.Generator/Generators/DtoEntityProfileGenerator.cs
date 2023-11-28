using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using WebAutoMapperProfile.Generator.CodeBuilders;

namespace WebAutoMapperProfile.Generator.Generators
{
    [Generator]
    public class DtoEntityProfileGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            try
            {
                var dtoEntityProfileBuilder = new DtoEntityProfileBuilder(context.Compilation.AssemblyName).Get(context);
                AddSource(context, "AutoMapper", dtoEntityProfileBuilder);
            }
            catch(Exception ex)
            {
                context.AddSource("Failed", ex.Message + " \n\nStacktrace:" + ex.StackTrace);
            }
        }
    }
}
