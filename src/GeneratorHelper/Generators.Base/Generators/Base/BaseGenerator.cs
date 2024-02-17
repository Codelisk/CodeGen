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
        protected void AddSourcewrong(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>> codeBuildersProvider)
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
                                fileName = fileName.Replace(" ", "leerzf").Replace(":", "dopspel").Replace(";", "Strichpunkt").Replace("@", "attet").Replace("{", "Klaamerauf").Replace("[", "eckigauf")
                                .Replace("}", "Klammerzu").Replace("|", "Oderdd").Replace("]", "eckigzua")
                                .Replace("<", "aufssa").Replace(">", "zueee").Replace(",", "Beispf").Replace("=", "gleichhh").Replace("_", "underrrr").Replace("(", "ra")
                                .Replace(")", "rzudsf").Replace("/", "backslash").Replace("-", "bindddd").Replace("?", "frageee").Replace("!", "ruffff").Replace("\r\n", "neuezeile").Replace("\n", "neuezeile");
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
    }
}
