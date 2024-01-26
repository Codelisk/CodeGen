using System.Runtime.InteropServices.ComTypes;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseGenerator : IIncrementalGenerator
    {
        public abstract void Initialize(IncrementalGeneratorInitializationContext context);

        protected void AddSource(IncrementalGeneratorInitializationContext context, Compilation compilation, string folderName, List<CodeBuilder> codeBuilders, (string, string)? replace = null)
        {
            foreach (var codeBuilder in codeBuilders)
            {
                codeBuilder.AddMissingNamespaceImports(compilation);
                string code = codeBuilder.Build();
                if (replace.HasValue)
                {
                    code = code.Replace(replace.Value.Item1, replace.Value.Item2);
                }

                if (codeBuilder.Classes.Any())
                {
                    //Workaround because i cant make Interface methods
                    if (codeBuilder.Classes.Any(x => x.Kind == TypeKind.Interface))
                    {
                        code = code.Replace("abstract ", "");
                        code = code.Replace("using <global namespace>;", "");
                    }

                    try
                    {
                        var fileName = codeBuilder.Classes.First().Name + ".g.cs";
                        if (string.IsNullOrEmpty(folderName))
                        {
                            context.RegisterPostInitializationOutput((c) =>
                            {
                                c.AddSource(fileName, code);
                            });
                        }
                        else
                        {
                            context.RegisterPostInitializationOutput((c) =>
                            {
                                c.AddSource(folderName + "/" + fileName, code);
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        context.RegisterPostInitializationOutput((c) =>
                        {
                            c.AddSource(folderName + "/Failed", ex.Message + " \n\nStacktrace:" + ex.StackTrace);
                        });
                    }
                }
            }
        }
    }
}
