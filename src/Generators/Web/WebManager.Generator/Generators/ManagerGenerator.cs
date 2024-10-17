using System.Collections.Immutable;
using System.Diagnostics;
using CodeGenHelpers;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using Codelisk.GeneratorAttributes.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Codelisk.GeneratorShared.Constants;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Extensions.New;
using Foundation.Crawler.Extensions.New.AttributeFinder;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Generators.Base.Generators.Base;
using Generators.Base.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebGenerator.Base;
using WebManager.Generator.CodeBuilders;

namespace WebManager.Generator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class ManagerGenerator : BaseGenerator
    {
        public override void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var dtos = context.Dtos();
            var managers = context.DefaultManagers();
            var repos = context.DefaultRepositories();
            var baseDtos = context.BaseDtos();

            var combinedResults = baseDtos.Combine(dtos).Combine(managers).Combine(repos);

            context.RegisterImplementationSourceOutput(
                combinedResults,
                static (sourceProductionContext, combinedResult) =>
                {
                    var baseDtos = combinedResult.Left.Left.Left; // baseDtos
                    var dtos = combinedResult.Left.Left.Right; // dtos
                    var managers = combinedResult.Left.Right; // managers
                    var repos = combinedResult.Right;

                    var result = new List<CodeBuilder?>();
                    foreach (var dto in dtos)
                    {
                        var builder = CodeBuilder.Create("Communalaudit.Api");
                        var baseManager = dto.TenantOrDefault<DefaultManagerAttribute>(managers);
                        var baseRepo = dto.TenantOrDefault<DefaultRepositoryAttribute>(repos);
                        Class(builder, dtos, dto, baseRepo, baseManager, baseDtos, managers, repos);
                        result.Add(builder);
                    }
                    var codeBuildersTuples = new List<(
                        List<CodeBuilder> codeBuilder,
                        string? folderName,
                        (string, string)? replace
                    )>
                    {
                        (result, "Managers", null),
                    };

                    AddSourceHelper.Add(sourceProductionContext, codeBuildersTuples);
                }
            );
        }

        private static ClassBuilder Class(
            CodeBuilder builder,
            IEnumerable<RecordDeclarationSyntax> dtos,
            RecordDeclarationSyntax dto,
            ClassDeclarationSyntax baseRepo,
            ClassDeclarationSyntax baseManager,
            IEnumerable<RecordDeclarationSyntax> baseDtos,
            ImmutableArray<ClassDeclarationSyntax> managers,
            ImmutableArray<ClassDeclarationSyntax> repos
        )
        {
            var dtoPropertiesWithForeignKey = dto.DtoForeignProperties(baseDtos);
            var constructedBaseManager = baseManager.Construct(dto);
            builder
                .AddClass("I" + dto.ManagerNameFromDto())
                .OfType(TypeKind.Interface)
                .SetBaseClass(
                    dto.ReplaceConstructValue(
                        constructedBaseManager.GetFirstInterfaceFullTypeName()
                    )
                )
                .WithAccessModifier(Accessibility.Public);

            var constructor = builder
                .AddClass(dto.ManagerNameFromDto())
                //Removed for performance.AddDtoUsing(context)
                //Removed for performance.AddEntityUsing(context, compilation)
                .WithAccessModifier(Accessibility.Public)
                .AddInterface("I" + dto.ManagerNameFromDto())
                .SetBaseClass(constructedBaseManager.GetFullTypeName())
                .AddAttribute(typeof(GeneratedManagerAttribute).FullName)
                .AddAttribute(typeof(RegisterTransient).FullName)
                .AddConstructor();

            List<(
                string repoType,
                string repoName,
                PropertyDeclarationSyntax propertySymbol,
                RecordDeclarationSyntax foreignKeyDto
            )> foreignRepos = new();
            foreach (var dtoProperty in dtoPropertiesWithForeignKey)
            {
                var foreignKeyName = dtoProperty.GetPropertyAttributeValue(
                    AttributeNames.ForeignKey
                );
                var foreignKeyDto = dtos.First(x => x.GetName() == foreignKeyName);
                string repoType = "I" + foreignKeyDto.ManagerNameFromDto();
                string repoName = foreignKeyDto.ManagerNameFromDto().GetParameterName();
                if (!foreignRepos.Any(x => x.repoType.Equals(repoType)))
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
                .AddParameterWithBaseCall(
                    "I" + dto.RepositoryNameFromDto(),
                    dto.RepositoryNameFromDto().GetParameterName()
                )
                .Class;

            foreach (var repo in foreignRepos)
            {
                result.AddProperty($"_{repo.Item2}").SetType(repo.Item1).WithReadonlyValue();
            }

            if (foreignRepos.Any())
            {
                //Generate GetFull methode
                {
                    var getMethode = baseRepo.GetMethodsWithAttributes<GetAttribute>(repos).First();
                    var getMethodeFull = baseManager
                        .GetMethodsWithAttributes<GetFullAttribute>(managers)
                        .First();
                    result
                        .AddMethod(ApiUrls.GetFull, Accessibility.Public)
                        .Override()
                        .WithReturnTypeTask("object")
                        .MakeAsync()
                        .AddParameter(
                            "Guid",
                            $"{dto.GetIdProperty(baseDtos).GetPropertyName().GetParameterName()}"
                        )
                        .WithBody(x =>
                        {
                            x.AppendLine(
                                $"{dto.GetFullModelName()} {dto.GetFullModelName()} = new ();"
                            );
                            x.AppendLine(
                                $"var {dto.GetName().GetParameterName()} = await {getMethode.GetName()}({dto.GetIdProperty(baseDtos).GetPropertyName().GetParameterName()});"
                            );
                            x.AppendLine(
                                $"{dto.GetFullModelName()}.{dto.GetName().GetParameterName()} = {dto.GetName().GetParameterName()};"
                            );
                            foreach (var repo in foreignRepos)
                            {
                                bool isNull = repo.propertySymbol.GetPropertyType().Contains("?");
                                string managerParametervalue = isNull
                                    ? repo.propertySymbol.GetPropertyName() + ".Value"
                                    : repo.propertySymbol.GetPropertyName();
                                string returnLine =
                                    $"{dto.GetFullModelName()}.{repo.propertySymbol.GetFullModelNameFromProperty()} = ({repo.foreignKeyDto.GetFullModelName()})await _{repo.repoName}.{getMethodeFull.GetName()}({dto.GetName().GetParameterName()}.{managerParametervalue});";
                                if (isNull)
                                {
                                    x.If(
                                            $"{dto.GetName().GetParameterName()}.{repo.propertySymbol.GetPropertyName()}.HasValue"
                                        )
                                        .WithBody(x =>
                                        {
                                            x.AppendLine(returnLine);
                                        })
                                        .EndIf();
                                }
                                else
                                {
                                    x.If(
                                            $"{dto.GetName().GetParameterName()}.{repo.propertySymbol.GetPropertyName()} != default"
                                        )
                                        .WithBody(x =>
                                        {
                                            x.AppendLine(returnLine);
                                        })
                                        .EndIf();
                                }
                            }

                            x.AppendLine($"return {dto.GetFullModelName()};");
                        })
                        .AddAttribute(typeof(GetFullAttribute).FullName);
                }

                {
                    string returnName = dto.GetFullModelName().GetParameterName() + "s";
                    //Generate GetAllFull methode
                    var getAllMethode = baseRepo
                        .GetMethodsWithAttributes<GetAllAttribute>(repos)
                        .First();
                    result
                        .AddMethod(ApiUrls.GetAllFull, Accessibility.Public)
                        .Override()
                        .WithReturnTypeTaskList("object")
                        .MakeAsync()
                        .WithBody(x =>
                        {
                            x.AppendLine($"List<object> {returnName} = new ();");
                            x.AppendLine(
                                $"var {dto.GetName().GetParameterName()}s = await {getAllMethode.GetName()}();"
                            );
                            x.ForEach(
                                    $"var {dto.GetName().GetParameterName()}",
                                    $"{dto.GetName().GetParameterName()}s"
                                )
                                .WithBody(x =>
                                {
                                    x.AppendLine(
                                        $"{returnName}.Add(await {ApiUrls.GetFull}({dto.GetName().GetParameterName()}.{dto.GetIdPropertyMethodeName(baseDtos)}));"
                                    );
                                });

                            x.AppendLine($"return {returnName};");
                        })
                        .AddAttribute(typeof(GetFullAttribute).FullName);
                }
            }

            //Add abstract methods
            result
                .AddMethod("ToDto", Accessibility.Public)
                .Override()
                .WithReturnType(dto.GetName())
                .AddParameter(dto.GetEntityName(), "entity")
                .WithBody(x => x.AppendLine("return entity.ToDto();"));
            result
                .AddMethod("ToDtos", Accessibility.Public)
                .Override()
                .WithReturnTypeList(dto.GetName())
                .AddParameter($"List<{dto.GetEntityName()}>", "entities")
                .WithBody(x => x.AppendLine("return entities.ToDtos();"));
            result
                .AddMethod("ToEntity", Accessibility.Public)
                .Override()
                .WithReturnType(dto.GetEntityName())
                .AddParameter(dto.GetName(), "dto")
                .WithBody(x => x.AppendLine("return dto.ToEntity();"));
            result
                .AddMethod("ToEntities", Accessibility.Public)
                .Override()
                .WithReturnTypeList(dto.GetEntityName())
                .AddParameter($"List<{dto.GetName()}>", "dtos")
                .WithBody(x => x.AppendLine("return dtos.ToEntities();"));
            return result;
        }
    }
}
