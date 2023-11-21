using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using CodeGenHelpers;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Models;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;

namespace Controller.Generator.CodeBuilders
{
    public class ControllerCodeBuilder : BaseCodeBuilder
    {
        public ControllerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
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
                var manager = context.Manager(dto);
                var baseController = context.Controller(dto);
                var builder = CreateBuilder();

                var c = Class(builder, dto, manager, baseController, context);

                Methods(c, dto, baseController, manager.GetClassWithMethods());

                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, INamedTypeSymbol manager, INamedTypeSymbol baseController, GeneratorExecutionContext context)
        {
            var constructedBaseController = baseController.ConstructFromDto(dto, context);
            return builder.AddClass(dto.ControllerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(constructedBaseController.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .BaseConstructorParameterBaseCall(constructedBaseController, (manager, dto.ManagerNameFromDto()))
                .Class;
        }
        private void Methods(ClassBuilder c, INamedTypeSymbol dto, INamedTypeSymbol baseController, ClassWithMethods repoModel)
        {
            var repoProperty = baseController.GetFieldsWithConstructedFromType(repoModel.Class).First();

            Dictionary<Type, string> methodsWithControllerAttributeName = new ()
            {
                {typeof(DeleteAttribute), Constants.HttpDeleteAttribute },
                {typeof(GetAttribute), Constants.HttpGetAttribute },
                {typeof(GetAllAttribute), Constants.HttpGetAttribute },
                {typeof(SaveAttribute), Constants.HttpPostAttribute },
            };

            if(dto.HasAttribute(nameof(RemoveGetAll)))
            {
                methodsWithControllerAttributeName.Remove(typeof(GetAllAttribute));
            }

            foreach (var item in methodsWithControllerAttributeName)
            {
                var method = repoModel.MethodFromAttribute(item.Key);
                var httpAttribute = method.HttpAttribute();
                var methodBuilder = c.AddMethod(method.MethodName(dto), Accessibility.Public)
                    .AddAttribute(method.HttpControllerAttribute(dto, item.Value))
                    .WithReturnTypeForHttpMethod(item.Key, dto)
                    .AddParametersForHttpMethod(httpAttribute, dto);

                if (item.Key == typeof(GetAllAttribute))
                {
                    if (dto.HasAttribute(nameof(CustomizeGetAll)))
                    {
                        bool anonymous = dto.GetAttribute<CustomizeGetAll>().NamedArguments.Any(x => x.Key.Equals("AllowAnonymous"));
                        if (anonymous)
                        {
                            methodBuilder.AddAttribute(Constants.AllowAnonymousAttribute);
                        }
                    }
                }

                methodBuilder.WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{httpAttribute.AttributeUrl(dto)}({httpAttribute.GetParametersNamesForHttpMethod(dto)});");
                });
            }
        }

    }
}
