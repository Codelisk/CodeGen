using System.Runtime.InteropServices.ComTypes;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Generators.Base
{
    public abstract class BaseGenerator : IIncrementalGenerator
    {
        public abstract void Initialize(IncrementalGeneratorInitializationContext context);


        protected void AddSourceImplementation(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>> codeBuildersProvider)
        {
            context.RegisterImplementationSourceOutput(codeBuildersProvider, static (sourceProductionContext, codeBuildersTuples) =>
            {
                foreach (var codeBuilderTuple in codeBuildersTuples)
                {
                    var codeBuilders = codeBuilderTuple.Item1;
                    string? folderName = codeBuilderTuple.Item2;
                    var replace = codeBuilderTuple.Item3;

                    foreach (var codeBuilder in codeBuilders)
                    {
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
                                    sourceProductionContext.AddSource(fileName, code);
                                }
                                else
                                {
                                    sourceProductionContext.AddSource(folderName + "/" + fileName, code);
                                }
                            }
                            catch (Exception ex)
                            {
                                sourceProductionContext.AddSource(folderName + "/Failed", ex.Message + " \n\nStacktrace:" + ex.StackTrace);
                            }
                        }
                    }
                }
            });
        }
        protected void AddSource(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>> codeBuildersProvider)
        {
            context.RegisterSourceOutput(codeBuildersProvider, static (sourceProductionContext, codeBuildersTuples) =>
            {
                foreach (var codeBuilderTuple in codeBuildersTuples)
                {
                    var codeBuilders = codeBuilderTuple.Item1;
                    string? folderName = codeBuilderTuple.Item2;
                    var replace = codeBuilderTuple.Item3;

                    foreach (var codeBuilder in codeBuilders)
                    {
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

                            code = code.Replace("internal void partial", "partial void");
                            code = code.Replace("void partial", "partial void");

                            try
                            {
                                var xyz = new Random().Next(999999);
                                var fileName = code + xyz + ".g.cs";
                                fileName.Replace(" ", "").Replace(":", "").Replace(";", "Strichpunkt").Replace("@", "at").Replace("{", "Klaamerauf").Replace("[","eckigauf")
                                .Replace("}", "Klammer zu").Replace("|", "Oder").Replace("]", "eckigzu")
                                .Replace("<","auf").Replace(">","zu").Replace(",","Beisp").Replace("=","gleich").Replace("_","under").Replace("(","ra")
                                .Replace(")","rzu");
                                if (string.IsNullOrEmpty(folderName))
                                {
                                    sourceProductionContext.AddSource(fileName, code);
                                }
                                else
                                {
                                    sourceProductionContext.AddSource(folderName + "/" + fileName, code);
                                }
                            }
                            catch (Exception ex)
                            {
                                sourceProductionContext.AddSource(folderName + "/Failed", ex.Message + " \n\nStacktrace:" + ex.StackTrace);
                            }
                        }
                    }
                }
            });
        }
    }
}
