using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Repository;
using Attributes.WebAttributes.Repository.Base;
using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Foundation.Crawler.Extensions.Extensions;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Generator.Generators.CodeBuilders
{
    public class ControllerCodeBuilder : BaseControllerCodeBuilder
    {
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

            Dictionary<IMethodSymbol, string> methodsWithControllerAttributeName = new Dictionary<IMethodSymbol, string>()
            {
                {repoModel.MethodFromAttribute<DeleteAttribute>(), "HttpDelete" },
                {repoModel.MethodFromAttribute<GetAttribute>(), "HttpGet" },
                {repoModel.MethodFromAttribute<GetAllAttribute>(), "HttpGet" },
                {repoModel.MethodFromAttribute<SaveAttribute>(), "HttpPost" },
            };

            foreach (var item in methodsWithControllerAttributeName)
            {
                var httpAttribute = item.Key.HttpAttribute();
                var methodBuilder = c.AddMethod(item.Key.MethodName(dto), Accessibility.Public)
                    .AddAttribute(item.Key.HttpControllerAttribute(dto, item.Value))
                    .WithReturnTypeForHttpMethod(httpAttribute, dto)
                    .AddParametersForHttpMethod(httpAttribute, dto);

                methodBuilder.WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{item.Key.Name}({httpAttribute.GetParametersNamesForHttpMethod(dto)});");
                });
            }
        }

    }
}
