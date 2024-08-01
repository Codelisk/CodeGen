using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Helpers
{
    public static class AddSourceHelper
    {
        public static void Add(
            SourceProductionContext sourceProductionContext,
            List<(
                List<CodeBuilder> codeBuilder,
                string? folderName,
                (string, string)? replace
            )> codeBuildersTuples
        )
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

                        var fileName = codeBuilder.Classes.First().Name;

                        AddCode(sourceProductionContext, code, folderName, fileName);
                    }
                }
            }
        }

        private static void AddCode(
            SourceProductionContext sourceProductionContext,
            string code,
            string folderName,
            string fileName
        )
        {
            //Workaround for
            //using Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserDto, Microsoft.AspNetCore.Identity.IdentityRole<System, System;
            //being included in DbContextGenerator usings
            string pattern = @"using\s+(global::)?[\w\.\+]+<[^;]*;?\s*|[\w\.\+]+<,*>";

            code = Regex.Replace(code, pattern, string.Empty, RegexOptions.Multiline);

            try
            {
                fileName = fileName + ".g.cs";
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
                sourceProductionContext.AddSource(
                    folderName + "/Failed",
                    ex.Message + " \n\nStacktrace:" + ex.StackTrace
                );
            }
        }
    }
}
