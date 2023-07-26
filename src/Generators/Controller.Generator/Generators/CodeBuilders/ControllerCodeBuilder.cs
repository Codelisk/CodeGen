using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
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
            var defaultRepository = context.DefaultRepository();
            var baseController = context.BaseController();
            return Build(dtos, defaultRepository, baseController);
        }

        private List<CodeBuilder?> Build(IEnumerable<INamedTypeSymbol> dtos, DefaultRepositoryModel repoModel, INamedTypeSymbol baseController)
        {
            var result = new List<CodeBuilder?>();
            foreach (var dto in dtos)
            {
                var builder = CreateBuilder();

                var c = Class(builder, dto, repoModel, baseController);

                Methods(c, dto, baseController, repoModel);

                result.Add(builder);
            }

            return result;
        }
        private ClassBuilder Class(CodeBuilder builder, INamedTypeSymbol dto, DefaultRepositoryModel repoModel, INamedTypeSymbol baseController)
        {
            return builder.AddClass(dto.ControllerNameFromDto()).WithAccessModifier(Accessibility.Public)
                .SetBaseClass(baseController.Construct(dto).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                .AddAttribute(Constants.ControllerAttribute)
                .AddConstructor()
                .WithBaseCall(new Dictionary<string, string> { { $"{repoModel.Repo.Name}<{dto.Name}>", repoModel.Repo.Name.GetParameterName() } })
                .Class;
        }
        private void Methods(ClassBuilder c, INamedTypeSymbol dto, INamedTypeSymbol baseController, DefaultRepositoryModel repoModel)
        {
            var repoProperty = baseController.GetFieldsWithConstructedFromType(repoModel.Repo).First();

            c.AddMethod(dto.DeleteMethodName(), Accessibility.Public)
                .AddAttribute(dto.DeleteAttribute())
                .AddParameter(dto.Name, dto.Name.GetParameterName())
                .WithReturnTypeTask()
                .WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{repoModel.Delete.Name}({dto.Name.GetParameterName()});");
                });


            c.AddMethod(dto.SaveMethodName(), Accessibility.Public)
                .AddAttribute(dto.PostAttribute())
                .AddParameter(dto.Name, dto.Name.GetParameterName())
                .WithReturnTypeTask(dto.Name)
                .WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{repoModel.Save.Name}({dto.Name.GetParameterName()});");
                });

            c.AddMethod(dto.GetMethodName(), Accessibility.Public)
                .AddAttribute(dto.GetAttribute())
                .AddParameter(dto.GetIdProperty().Type.Name, dto.GetIdProperty().Name.GetParameterName())
                .WithReturnTypeTask(dto.Name)
                .WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{repoModel.Get.Name}(id);");
                });

            c.AddMethod(dto.GetAllMethodName(), Accessibility.Public)
                .AddAttribute(dto.GetAttribute(true))
                .WithReturnTypeTaskList(dto.Name)
                .WithBody((x) =>
                {
                    x.AppendLine($"return {repoProperty.Name}.{repoModel.GetAll.Name}();");
                });
        }

    }
}
