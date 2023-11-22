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
            var dtoPropertiesWithForeignKey = dto.GetAllProperties().Where(x => x.GetAllAttributes().Any(x=>x.AttributeClass.Name.Contains("Foreig")) || x.HasAttribute("System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute"));
            var constructedBaseManager = baseManager.ConstructFromDto(dto, context);

            var constructor = builder.AddClass(dto.ManagerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.ManagerNameFromDto())
                .SetBaseClass(constructedBaseManager.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(typeof(GeneratedManagerAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor();

            List<(string, string)> foreignRepos = new();
            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetAllAttributes().First(x => x.AttributeClass.Name.Contains("Foreig")).ConstructorArguments.First().Value.ToString();
                var foreignKeyDto = context.Dtos().First(x => x.Name == foreignKeyName);
                string repoType = "I" + foreignKeyDto.RepositoryNameFromDto();
                string repoName = foreignKeyDto.RepositoryNameFromDto().GetParameterName();
                foreignRepos.Add((repoType, repoName));
            }
            
            foreach (var repo in foreignRepos)
            {
                constructor.AddParameter(repo.Item1, repo.Item2);
            }

            constructor.WithBody(x =>
            {
                foreach (var repo in foreignRepos)
                {
                    /*result.AddProperty($"_{repo.Item2}").SetType(repo.Item1).WithReadonlyValue()
                        .WithAccessModifier(Accessibility.Private);*/
                    x.AppendLine($"_{repo.Item2} = {repo.Item2};");
                }
            });
            
            var result = constructor
                .BaseConstructorParameterBaseCall(constructedBaseManager, (baseRepo, dto.RepositoryNameFromDto()))
                .Class;

            foreach (var repo in foreignRepos)
            {
                result.AddProperty($"_{repo.Item2}").SetType(repo.Item1).WithReadonlyValue()
                    .WithAccessModifier(Accessibility.Private);
            }

            return result;
        }

    }
}
