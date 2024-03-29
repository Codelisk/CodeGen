using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;

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

                            //Workaround for interface generation in pipeline always generating false returntype for AddRange in GenerateInterface
                            string pattern = @"System\+Collections\+Generic\+List`1\[(.*?)\]";
                            string replacement = "System.Collections.Generic.List<$1>";

                            string corrected = Regex.Replace(code, pattern, replacement);

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

                            //Workaround for interface generation in pipeline always generating false returntype for AddRange in GenerateInterface
                            //string pattern = @"System\+Collections\+Generic\+List`1\[(.*?)\]";
                            //string replacement = "List<$1>";

                            //string corrected = Regex.Replace(code, pattern, replacement);

                            //Workaround for 
                            //using Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserDto, Microsoft.AspNetCore.Identity.IdentityRole<System, System;
                            //being included in DbContextGenerator usings
                            string pattern = @"using\s+(global::)?[\w\.\+]+<[^;]*;?\s*|[\w\.\+]+<,*>";

                            code = Regex.Replace(code, pattern, string.Empty, RegexOptions.Multiline);

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
        protected void AddSourceFileName(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<List<(List<CodeBuilder> codeBuilder, string? folderName, (string, string)? replace)>> codeBuildersProvider)
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

                            //Workaround for interface generation in pipeline always generating false returntype for AddRange in GenerateInterface
                            string pattern = @"System\+Collections\+Generic\+List`1\[(.*?)\]";
                            string replacement = "List<$1>";

                            code = Regex.Replace(code, pattern, replacement);

                            //Workaround for 
                            //using Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserDto, Microsoft.AspNetCore.Identity.IdentityRole<System, System;
                            //being included in DbContextGenerator usings
                            pattern = @"using\s+(global::)?[\w\.]+<.*?(>;|$)";

                            code = Regex.Replace(code, pattern, string.Empty, RegexOptions.Multiline);


                            try
                            {
                                var fileName = code;

                                fileName = fileName.Replace(" ", "leerzf")
.Replace(":", "dopsspel")
.Replace(";", "Strichpunkt")
.Replace("@", "atteit")
.Replace("{", "Kliaamerauf")
.Replace("[", "eckiigauf")
.Replace("}", "Klammerzu")
.Replace("|", "Od�erdd")
.Replace("]", "eclkigzua")
.Replace("<", "aufkssa")
.Replace(">", "zusjeee")
.Replace(",", "Beihspf")
.Replace("=", "gleigchhh")
.Replace("_", "underrrrr")
.Replace("(", "radsfsaaf")
.Replace(")", "rzuwdsf")
.Replace("/", "backaslash")
.Replace("-", "bindaddd")
.Replace("?", "frageee")
.Replace("!", "rufffff")
.Replace("\r\n", "neuezeile")
.Replace("\n", "neuezeile") + ".g.cs";


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
