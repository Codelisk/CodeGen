using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;
using System.Diagnostics;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryCodeBuilder : BaseCodeBuilder
    {
        public RepositoryCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
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
            foreach (var dto in dtos)
            {
                var builder = CreateBuilder();
                var baseRepository = context.Repository(dto);
                Class(builder, dto, baseRepository, context);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol baseRepository, GeneratorExecutionContext context)
        {
            var constructedBaseRepo = baseRepository.ConstructFromDto(dto, context);
            return builder.AddClass(dto.RepositoryNameFromDto()).WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.RepositoryNameFromDto())
                .SetBaseClass(constructedBaseRepo.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(typeof(GeneratedRepositoryAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseRepo)
                .Class;
        }
    }
}
