using Attributes;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextCodeBuilder : BaseCodeBuilder
    {
        public DbContextCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var dtos = context.Dtos().ToList();
            return Build(context, dtos);
        }

        private List<CodeBuilder?> Build(GeneratorExecutionContext context, IEnumerable<INamedTypeSymbol> dtos)
        {
            var result = new List<CodeBuilder?>();
            foreach (var groupedDtos in dtos.GroupBy(x => x.GetAttribute<DtoAttribute>().GetFirstConstructorArgument()))
            {
                var baseContext = context.BaseContext();
                var builder = CreateBuilder(baseContext.ContainingNamespace.ToString());
                Class(builder, groupedDtos, baseContext, context);
                result.Add(builder);
            }

            return result;
        }
        private IReadOnlyList<ClassBuilder> Class(CodeBuilder builder, IGrouping<string, INamedTypeSymbol> dtos, INamedTypeSymbol baseContext, GeneratorExecutionContext context)
        {
            var result = builder.AddClass(dtos.Key).WithAccessModifier(Accessibility.Public).AddNamespaceImport("Microsoft.EntityFrameworkCore")
                .AddAttribute(nameof(GeneratedDbContextAttribute));

            foreach (var dto in dtos)
            {
                result.AddProperty(dto.Name.GetParameterName(), Accessibility.Public).SetType($"DbSet<{dto.Name}>").UseAutoProps();
            }

            return builder.Classes;
        }

    }
}