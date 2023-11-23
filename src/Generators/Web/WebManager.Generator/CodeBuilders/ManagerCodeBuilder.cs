using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using WebGenerator.Base;
using Shared.Constants;
using Foundation.Crawler.Extensions;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Foundation.Crawler.Extensions.Extensions;

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
            var dtoPropertiesWithForeignKey = dto.DtoForeignProperties();
            var constructedBaseManager = baseManager.ConstructFromDto(dto, context);

            var constructor = builder.AddClass(dto.ManagerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.ManagerNameFromDto())
                .SetBaseClass(constructedBaseManager.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(typeof(GeneratedManagerAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor();

            List<(string repoType, string repoName, IPropertySymbol propertySymbol, INamedTypeSymbol foreignKeyDto)> foreignRepos = new();
            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetPropertyAttributeValue(AttributeNames.ForeignKey);
                var foreignKeyDto = context.Dtos().First(x => x.Name == foreignKeyName);
                string repoType = "I" + foreignKeyDto.ManagerNameFromDto();
                string repoName = foreignKeyDto.ManagerNameFromDto().GetParameterName();
                if(!foreignRepos.Any(x=>x.repoType.Equals(repoType)))
                {
                    foreignRepos.Add((repoType, repoName, dtoProperty, foreignKeyDto));
                }
            }
            
            foreach (var repo in foreignRepos)
            {
                constructor.AddParameter(repo.Item1, repo.Item2);
            }

            constructor.WithBody(x =>
            {
                foreach (var repo in foreignRepos)
                {
                    x.AppendLine($"_{repo.Item2} = {repo.Item2};");
                }
            });

            var result = constructor
                .BaseConstructorParameterBaseCall(constructedBaseManager, (baseRepo, dto.RepositoryNameFromDto()))
                .Class;

            foreach (var repo in foreignRepos)
            {
                result.AddProperty($"_{repo.Item2}").SetType(repo.Item1).WithReadonlyValue();
            }

            //if (foreignRepos.Any())
            //{
                //Generate GetFull methode
                {
                    var getMethode = baseRepo.GetMethodsWithAttribute(nameof(Codelisk.GeneratorAttributes.WebAttributes.HttpMethod.GetAttribute)).First();
                    var getMethodeFull = baseManager.GetMethodsWithAttribute(nameof(Codelisk.GeneratorAttributes.WebAttributes.HttpMethod.GetFullAttribute)).First();
                    result.AddMethod(ApiUrls.GetFull, Accessibility.Public).Override().WithReturnTypeTask("object").MakeAsync().AddParametersForHttpMethod(getMethode.HttpAttribute(), dto).WithBody(x =>
                {
                    x.AppendLine($"{dto.GetFullModelName()} {dto.GetFullModelName()} = new ();");
                    x.AppendLine($"var {dto.Name.GetParameterName()} = await {getMethode.Name}({dto.GetIdProperty().Name.GetParameterName()});");
                    x.AppendLine($"{dto.GetFullModelName()}.{dto.Name.GetParameterName()} = {dto.Name.GetParameterName()};");
                    foreach (var repo in foreignRepos)
                    {
                        bool isPropertyNullable = repo.propertySymbol.NullableAnnotation == NullableAnnotation.Annotated;
                        string managerParametervalue = isPropertyNullable ? repo.propertySymbol.Name + ".Value" : repo.propertySymbol.Name;
                        string returnLine = $"{dto.GetFullModelName()}.{repo.propertySymbol.GetFullModelNameFromProperty()} = ({repo.foreignKeyDto.GetFullModelName()})await _{repo.repoName}.{getMethodeFull.Name}({dto.Name.GetParameterName()}.{managerParametervalue});";
                        if (isPropertyNullable)
                        {
                            x.If($"{dto.Name.GetParameterName()}.{repo.propertySymbol.Name}.HasValue").WithBody(x =>
                            {
                                x.AppendLine(returnLine);
                            }).EndIf();
                        }
                        else
                        {
                            x.AppendLine(returnLine);
                        }
                    }

                    x.AppendLine($"return {dto.GetFullModelName()};");
                }).AddAttribute(nameof(Codelisk.GeneratorAttributes.WebAttributes.HttpMethod.GetFullAttribute));
                }

                {
                    string returnName= dto.GetFullModelName().GetParameterName() + "s";
                    //Generate GetAllFull methode
                    var getAllMethode = baseRepo.GetMethodsWithAttribute(nameof(Codelisk.GeneratorAttributes.WebAttributes.HttpMethod.GetAllAttribute)).First();
                    result.AddMethod(ApiUrls.GetAllFull, Accessibility.Public).Override().WithReturnTypeTaskList("object").MakeAsync().WithBody(x =>
                    {
                        x.AppendLine($"List<object> {returnName} = new ();");
                        x.AppendLine($"var {dto.Name.GetParameterName()}s = await {getAllMethode.Name}();");
                        x.ForEach($"var {dto.Name.GetParameterName()}", $"{dto.Name.GetParameterName()}s").WithBody(x =>
                        {
                            x.AppendLine($"{returnName}.Add(await {ApiUrls.GetFull}({dto.Name.GetParameterName()}.{dto.GetIdPropertyMethodeName()}));");
                        });

                        x.AppendLine($"return {returnName};");
                    }).AddAttribute(nameof(Codelisk.GeneratorAttributes.WebAttributes.HttpMethod.GetFullAttribute));
                }
            //}

            return result;
        }

    }
}
