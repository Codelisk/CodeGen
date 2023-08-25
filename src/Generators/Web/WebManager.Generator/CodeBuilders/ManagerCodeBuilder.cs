using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;

namespace WebManager.Generator.CodeBuilders
{
    public class ManagerCodeBuilder : BaseCodeBuilder
    {
        public ManagerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
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
                var baseManager = context.Manager(dto);
                Class(builder, dto, context.Repository(dto), baseManager, context);
                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol baseRepo, INamedTypeSymbol baseManager, GeneratorExecutionContext context)
        {
            var constructedBaseManager = baseManager.ConstructFromDto(dto, context);
            return builder.AddClass(dto.ManagerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.ManagerNameFromDto())
                .SetBaseClass(constructedBaseManager.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(typeof(GeneratedManagerAttribute).FullName)
                .AddAttribute(AutoRegisterInject.Constants.RegisterAttributeContants.TRANSIENT_ATTRIBUTE_NAME)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseManager, (baseRepo, dto.RepositoryNameFromDto()))
                .Class;
        }

    }
}
