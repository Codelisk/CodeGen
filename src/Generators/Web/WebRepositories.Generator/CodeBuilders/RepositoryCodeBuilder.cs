using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;
using System.Diagnostics;
using Foundation.Crawler.Extensions;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryCodeBuilder : BaseCodeBuilder
    {
        public RepositoryCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {

        }

        public override List<CodeBuilder> Get(Compilation compilation, List<CodeBuilder> codeBuilders = null)
        {
            var attributeCompilationCrawler = new AttributeCompilationCrawler(compilation);
            var dtos = attributeCompilationCrawler.Dtos().ToList();
            return Build(attributeCompilationCrawler, compilation, dtos);
        }

        private List<CodeBuilder?> Build(AttributeCompilationCrawler attributeCompilationCrawler, Compilation compilation, IEnumerable<INamedTypeSymbol> dtos)
        {
            var result = new List<CodeBuilder?>();
            foreach (var dto in dtos)
            {
                var builder = CreateBuilder();
                var baseRepository = attributeCompilationCrawler.Repository(dto);
                Class(builder, dto, baseRepository, compilation);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol baseRepository, Compilation context)
        {
            var constructedBaseRepo = baseRepository.ConstructFromDto(dto, context);
            return builder.AddClass(dto.RepositoryNameFromDto()).WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.RepositoryNameFromDto())
                .SetBaseClass(constructedBaseRepo.GetFullTypeName())
                .AddAttribute(typeof(GeneratedRepositoryAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseRepo)
                .Class;
        }
    }
}
