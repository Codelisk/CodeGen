using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Foundation.Crawler.Crawlers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Api.Generator.Generators.CodeBuilders.FromController
{
    public class RefitApisFromControllerCodeBuilder : BaseWebApiCodeBuilder
    {
        public RefitApisFromControllerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var result = new List<CodeBuilder>();
            var controllers = GetControllers(context).ToList();
            foreach (var controller in controllers)
            {
                var codeBuilder = CreateBuilder();
                GenerateRefitApis(context, controller, codeBuilder);
                result.Add(codeBuilder);
            }

            return result;
        }
        public void GenerateRefitApis(GeneratorExecutionContext context, INamedTypeSymbol controller, CodeBuilder codeBuilder)
        {
            var methodsWithAttributes = controller.GetMethods();

            if (methodsWithAttributes == null || !methodsWithAttributes.GetEnumerator().MoveNext())
                return;

            string className = controller.RefitInterfaceNameFromController();

            var apiInterface = codeBuilder.AddClass(className).WithAccessModifier(Accessibility.Public).OfType(TypeKind.Interface).Abstract(false)
                .AddInterface(context.BaseApi());

            foreach (var method in methodsWithAttributes)
            {
                GenerateRefitApiMethod(context, apiInterface, method);
            }

            //context.AddSource(className, codeBuilder.Build().Replace("abstract partial", "partial"));
        }

        private void GenerateRefitApiMethod(GeneratorExecutionContext context, ClassBuilder apiInterface, IMethodSymbol method)
        {
            AttributeData attribut = null;

            var routeAttribute = method.GetAttributes().GetAttributeWithName("RouteAttribute");
            var httpGetMethodAttribute = method.GetAttributes().GetAttributeWithName("HttpGetAttribute");
            var httpPostMethodAttribute = method.GetAttributes().GetAttributeWithName("HttpPostAttribute");
            var httpPutMethodAttribute = method.GetAttributes().GetAttributeWithName("HttpPutAttribute");
            var httpDeleteMethodAttribute = method.GetAttributes().GetAttributeWithName("HttpDeleteAttribute");

            if (routeAttribute is not null)
            {
                attribut = routeAttribute;
            }
            else if (httpGetMethodAttribute is not null)
            {
                attribut = httpGetMethodAttribute;
            }
            else if (httpPostMethodAttribute is not null)
            {
                attribut = httpPostMethodAttribute;
            }
            else if (httpPutMethodAttribute is not null)
            {
                attribut = httpPutMethodAttribute;
            }
            else if (httpDeleteMethodAttribute is not null)
            {
                attribut = httpDeleteMethodAttribute;
            }

            string routeTemplate = attribut?.ConstructorArguments.FirstOrDefault().Value as string;
            if (!string.IsNullOrEmpty(routeTemplate))
            {
                var returnType = method.GetReturnTypeOrString(context, false);


                if (httpGetMethodAttribute != null)
                {
                    GenerateFromGet(method, apiInterface, routeTemplate, returnType);
                }
                else if (httpPostMethodAttribute != null)
                {
                    GenerateFromPost(method, apiInterface, routeTemplate, returnType);
                }
                else if (httpPutMethodAttribute != null)
                {
                    GenerateFromPut(method, apiInterface, routeTemplate, returnType);
                }
                else if (httpDeleteMethodAttribute != null)
                {
                    GenerateFromDelete(method, apiInterface, routeTemplate, returnType);
                }
            }
        }
        private void GenerateFromPut(IMethodSymbol method, ClassBuilder apiInterface, string routeTemplate, ITypeSymbol returnType)
        {
            var methodName = method.Name;

            var methodDeclaration = apiInterface.AddMethod(methodName, Accessibility.NotApplicable)
                .AddAttribute($"[Put(\"/{routeTemplate}\")]")
                .WithReturnTypeTask(returnType.GetReturnTypeName()).Abstract(true);

            foreach (var parameter in method.Parameters)
            {
                var parameterType = AddBodyOrQueryToType(parameter);

                var parameterName = parameter.Name;

                methodDeclaration.AddParameter(parameterType, parameterName);
            }
        }
        private void GenerateFromDelete(IMethodSymbol method, ClassBuilder apiInterface, string routeTemplate, ITypeSymbol returnType)
        {
            var methodName = method.Name;

            var methodDeclaration = apiInterface.AddMethod(methodName, Accessibility.NotApplicable)
                .AddAttribute($"[Delete(\"/{routeTemplate}\")]")
                .WithReturnTypeTask(returnType.GetReturnTypeName()).Abstract(true);

            foreach (var parameter in method.Parameters)
            {
                var parameterType = AddBodyOrQueryToType(parameter);
                var parameterName = parameter.Name;

                methodDeclaration.AddParameter(parameterType, parameterName);
            }
        }
        private void GenerateFromGet(IMethodSymbol method, ClassBuilder apiInterface, string routeTemplate, ITypeSymbol returnType)
        {
            var methodName = method.Name;

            var methodDeclaration = apiInterface.AddMethod(methodName, Accessibility.NotApplicable)
                .AddAttribute($"[Get(\"/{routeTemplate}\")]")
                .WithReturnTypeTask(returnType.GetReturnTypeName()).Abstract(true);

            foreach (var parameter in method.Parameters)
            {
                var parameterType = AddBodyOrQueryToType(parameter);
                var parameterName = parameter.Name;

                methodDeclaration.AddParameter(parameterType, parameterName);
            }
        }

        private void GenerateFromPost(IMethodSymbol method, ClassBuilder apiInterface, string routeTemplate, ITypeSymbol returnType)
        {
            var methodName = method.Name;

            var methodDeclaration = apiInterface.AddMethod(methodName, Accessibility.NotApplicable)
                .AddAttribute($"[Post(\"/{routeTemplate}\")]")
                .WithReturnTypeTask(returnType.GetReturnTypeName()).Abstract(true);

            foreach (var parameter in method.Parameters)
            {
                var parameterType = AddBodyOrQueryToType(parameter);
                var parameterName = parameter.Name;

                methodDeclaration.AddParameter(parameterType, parameterName);
            }
        }
        private string AddBodyOrQueryToType(IParameterSymbol parameter)
        {
            var parameterType = parameter.Type.ToDisplayString();
            if (parameter.HasAttribute("FromQueryAttribute"))
            {
                parameterType = "[Query] " + parameterType;
            }
            if (parameter.HasAttribute("FromBodyAttribute"))
            {
                parameterType = "[Body] " + parameterType;
            }
            return parameterType;
        }

    }
}
