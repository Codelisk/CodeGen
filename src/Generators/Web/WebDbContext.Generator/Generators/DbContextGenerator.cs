using System.Collections.Immutable;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.New;
using Foundation.Crawler.Extensions.New.AttributeFinder;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebDbContext.Generator.CodeBuilders;

namespace WebDbContext.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class DbContextGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var dbcontext = context.DbContexts();
            var baseDtosAndClasses = context.BaseDtos().Combine(dtos).Combine(dbcontext);
            context.RegisterImplementationSourceOutput(
                baseDtosAndClasses,
                static (sourceProductionContext, combinedResult) =>
                {
                    // Hier kannst du die kombinierten Ergebnisse verarbeiten
                    var (baseDtos, dtos) = combinedResult.Left;
                    var dbContexts = combinedResult.Right;

                    var result = new List<CodeBuilder?>();
                    var nameSpace = dbContexts.First().GetNamespace();
                    var builder = CodeBuilder.Create(nameSpace);
                    Class(builder, dtos, dbContexts.First());
                    result.Add(builder);
                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "DbContext", null),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static IReadOnlyList<ClassBuilder> Class(
            CodeBuilder builder,
            ImmutableArray<RecordDeclarationSyntax> entities,
            ClassDeclarationSyntax baseContext
        )
        {
            var result = builder
                .AddClass(baseContext.GetName())
                .WithAccessModifier(Accessibility.Public)
                .SetBaseClass(baseContext.GetBaseClassName())
                .AddNamespaceImport("Microsoft.EntityFrameworkCore")
                .AddAttribute(typeof(GeneratedDbContextAttribute).FullName);

            result
                .AddMethod("partial GeneratedModelCreating")
                .AddParameter("ModelBuilder", "modelBuilder")
                .WithBody(x =>
                {
                    foreach (var entity in entities)
                    {
                        x.AppendLine($"modelBuilder.Entity<{entity.GetEntityName()}>();");
                    }
                });

            foreach (var entity in entities)
            {
                result
                    .AddProperty(entity.GetEntityName().GetParameterName(), Accessibility.Public)
                    .SetType($"DbSet<{entity.GetEntityName()}>")
                    .UseAutoProps();
            }

            return builder.Classes;
        }
    }
}
