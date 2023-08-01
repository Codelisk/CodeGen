using CodeGenHelpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseGenerator : ISourceGenerator
    {
        protected void AddSource(GeneratorExecutionContext context, string folderName, List<CodeBuilder> codeBuilders, (string, string)? replace = null)
        {
            foreach (var codeBuilder in codeBuilders)
            {
                string code = codeBuilder.Build();
                if(replace.HasValue)
                {
                    code = code.Replace(replace.Value.Item1, replace.Value.Item2);
                }

                if (codeBuilder.Classes.Any())
                {
                    //Workaround because i cant make Interface methods
                    if(codeBuilder.Classes.Any(x=>x.Kind == TypeKind.Interface))
                    {
                        code = code.Replace("abstract ", "");
                        code = code.Replace("using <global namespace>;", "");
                    }

                    try
                    {
                        var fileName = codeBuilder.Classes.First().Name + ".g.cs";
                        if (string.IsNullOrEmpty(folderName))
                        {
                            context.AddSource(fileName, code);
                        }
                        else
                        {
                            context.AddSource(folderName + "/" + fileName, code);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        public abstract void Execute(GeneratorExecutionContext context);

        public virtual void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
