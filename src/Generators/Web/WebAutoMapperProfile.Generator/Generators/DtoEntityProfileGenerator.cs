using System.Diagnostics;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.New;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;

namespace WebAutoMapperProfile.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class DtoEntityProfileGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var baseDtosAndClasses = context.BaseDtos().Combine(dtos);
            context.RegisterImplementationSourceOutput(
                baseDtosAndClasses,
                static (sourceProductionContext, baseDtosAndClasses) =>
                {
                    var dtos = baseDtosAndClasses.Right;
                    var result = new List<CodeBuilder?>();
                    var nameSpace = dtos.First().GetNamespace();

                    var builder = CodeBuilder.Create(nameSpace);
                    builder
                        .AddClass(Constants.ProfileName)
                        .WithAccessModifier(Accessibility.Public)
                        .SetBaseClass(Constants.AutoMapperProfileBaseClass)
                        .AddNamespaceImport(Constants.AutoMapperNamespaceImport)
                        .AddConstructor()
                        .WithBody(x =>
                        {
                            foreach (var d in dtos)
                            {
                                x.AppendLine(
                                    $"{Constants.AutoMapperCreateMap}<{d.GetName()}, {d.GetEntityName()}>().{Constants.AutoMapperReverseMap}();"
                                );
                                //x.AppendLine($"{Constants.AutoMapperCreateMap}<List<{d.Name}>, List<{d.EntityFromDto(context).Name}>>().{Constants.AutoMapperReverseMap}();");
                            }
                        });

                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "AutoMapper", null),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }
    }
}
