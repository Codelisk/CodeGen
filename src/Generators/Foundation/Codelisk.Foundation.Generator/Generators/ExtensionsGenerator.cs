using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.New;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codelisk.Foundation.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    internal class ExtensionsGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            context.RegisterSourceOutput(
                dtos,
                static (sourceProductionContext, dtos) =>
                {
                    var result = new List<CodeBuilder?>();
                    var nameSpace = dtos.First().GetNamespace();

                    var builder = CodeBuilder.Create(nameSpace);
                    Class(builder, dtos);
                    result.Add(builder);

                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "Extensions", ("namespace <global namespace>;", "")),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static List<CodeBuilder?> Class(
            CodeBuilder builder,
            ImmutableArray<ClassDeclarationSyntax> dtos
        )
        {
            var result = new List<CodeBuilder?>();

            var extensionsClass = builder
                .AddClass("DtoEntityExtensions")
                .MakeStaticClass()
                //Removed for performance.AddDtoUsing(context)
                .MakePublicClass();

            foreach (var dto in dtos)
            {
                var properties = dto.GetAllProperties(true, true);
                var dtoName = dto.GetName();
                var entityName = dto.GetEntityName();
                extensionsClass
                    .AddMethod("ToEntity", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + dtoName, dtoName.GetParameterName())
                    .WithBody(x =>
                    {
                        x.AppendLine(
                            $"var result = new {entityName}({dtoName.GetParameterName()});"
                        );
                        x.AppendLine("return result;");
                    })
                    .WithReturnType(entityName);

                extensionsClass
                    .AddMethod("ToEntities", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + $"List<{dtoName}>", dtoName.GetParameterName(true))
                    .WithBody(x =>
                    {
                        x.AppendLine($"var result = new List<{entityName}>();");
                        x.ForEach("var dto", dtoName.GetParameterName(true))
                            .WithBody(y =>
                            {
                                y.AppendLine($"result.Add(new {entityName}(dto));");
                            });
                        x.AppendLine("return result;");
                    })
                    .WithReturnTypeList(entityName);

                extensionsClass
                    .AddMethod("ToDto", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + entityName, "entity")
                    .WithBody(x =>
                    {
                        x.AppendLine($"return entity as {dtoName};");
                    })
                    .WithReturnType(dtoName);

                extensionsClass
                    .AddMethod("ToDtos", Accessibility.Public)
                    .MakeStaticMethod()
                    .AddParameter("this " + $"List<{entityName}>", "entities")
                    .WithBody(x =>
                    {
                        x.AppendLine($"var result = new List<{dtoName}>();");
                        x.ForEach("var entity", "entities")
                            .WithBody(y =>
                            {
                                y.AppendLine($"result.Add(entity as {dtoName});");
                            });
                        x.AppendLine("return result;");
                    })
                    .WithReturnTypeList(dtoName);
            }

            result.Add(builder);
            return result;
        }
    }
}
