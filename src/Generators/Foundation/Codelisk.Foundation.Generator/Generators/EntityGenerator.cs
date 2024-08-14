using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.New;
using Generators.Base.CodeBuilders;
using Generators.Base.Extensions;
using Generators.Base.Extensions.Common;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codelisk.Foundation.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    internal class EntityGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var compilationAndClasses = context.CompilationProvider.Combine(dtos);
            context.RegisterImplementationSourceOutput(
                compilationAndClasses,
                static (sourceProductionContext, compilationAndClasses) =>
                {
                    var dtos = compilationAndClasses.Right;
                    var result = new List<CodeBuilder?>();
                    var nameSpace = dtos.First().GetNamespace();
                    foreach (var dto in dtos)
                    {
                        var builder = CodeBuilder.Create(nameSpace);
                        Class(builder, dto, compilationAndClasses.Left);
                        result.Add(builder);
                    }
                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "Entities", ("namespace <global namespace>;", "")),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static IReadOnlyList<ClassBuilder> Class(
            CodeBuilder builder,
            ClassDeclarationSyntax dto,
            Compilation compilation
        )
        {
            var result = builder
                .TopLevelNamespace()
                .AddClass(dto.GetEntityName())
                .SetBaseClass(dto.Identifier.Text)
                .AddAttribute($"{typeof(EntityAttribute).FullName}(typeof({dto.Identifier.Text}))")
                .WithAccessModifier(Accessibility.Public);

            result.AddConstructor();
            var constructor = result.AddConstructor().AddParameter(dto.Identifier.Text);
            var semanticModel = compilation.GetSemanticModel(dto.SyntaxTree);
            var properties = dto.GetAllPropertiesWithBaseClass(semanticModel, true);

            constructor.WithBody(x =>
            {
                foreach (var property in properties)
                {
                    x.AppendLine(
                        $"this.{property.Name} = {dto.GetName().GetParameterName()}.{property.Name};"
                    );
                }
            });

            return builder.Classes;
        }
    }
}
