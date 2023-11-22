using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.Foundation.Generator.CodeBuilders;
using Generators.Base.Generators.Base;
using Microsoft.CodeAnalysis;

namespace Codelisk.Foundation.Generator.Generators
{
    [Generator]
    public class FullModelGenerator : BaseGenerator
    {
        public override void Execute(GeneratorExecutionContext context)
        {
            if (context.Compilation.AssemblyName.Contains("Generator"))
            {
                return;
            }
            try
            {
                var fullModelCodeBuilder = new FullModelCodeBuilder(context.Compilation.AssemblyName).Get(context);
                AddSource(context, "FullModels", fullModelCodeBuilder);
            }
            catch(Exception ex)
            {
                context.AddSource("ErrorFullModelGenerator", $"//{ex.Message} \n\n {ex.StackTrace}");
            }
}
    }
}
