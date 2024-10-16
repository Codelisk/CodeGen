using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorShared.Constants;
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
    public class FullModelGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var baseDtosAndClasses = context.BaseDtos().Combine(dtos);
            context.RegisterImplementationSourceOutput(
                baseDtosAndClasses,
                static (sourceProductionContext, baseDtosAndClasses) =>
                {
                    var result = new List<CodeBuilder?>();
                    foreach (var dto in baseDtosAndClasses.Right)
                    {
                        var builder = CodeBuilder.Create(dto.GetNamespace());
                        Class(builder, dto, baseDtosAndClasses.Right, baseDtosAndClasses.Left);
                        result.Add(builder);
                    }
                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "FullModels", ("namespace <global namespace>;", "")),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static IReadOnlyList<ClassBuilder> Class(
            CodeBuilder builder,
            RecordDeclarationSyntax dto,
            ImmutableArray<RecordDeclarationSyntax> dtos,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            var result = builder
                .TopLevelNamespace()
                .AddClass(dto.GetFullModelName())
                .WithAccessModifier(Accessibility.Public);

            result
                .AddProperty(dto.GetName().GetParameterName(), Accessibility.Public)
                .SetType(dto.GetName())
                .UseAutoProps();

            var dtoPropertiesWithForeignKey = dto.DtoForeignProperties(baseDtos);

            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetPropertyAttributeValue(
                    AttributeNames.ForeignKey
                );
                var foreignKeyDto = dtos.First(x => x.GetName() == foreignKeyName);

                result
                    .AddProperty(dtoProperty.GetFullModelNameFromProperty(), Accessibility.Public)
                    .SetType(foreignKeyDto.GetFullModelName())
                    .UseAutoProps();
            }

            return builder.Classes;
        }
    }
}
