using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New.AttributeFinder
{
    public static class ControllerAttributeFinderExtensions
    {
        public static string MethodName(
            this MethodDeclarationSyntax method,
            RecordDeclarationSyntax dto
        )
        {
            var attribute = method.HttpAttribute().First();
            return attribute.GetFirstConstructorArgument();
        }

        public static IncrementalValueProvider<
            ImmutableArray<ClassDeclarationSyntax>
        > DefaultControllers(this IncrementalGeneratorInitializationContext context)
        {
            var defaultControllers = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DefaultControllerAttribute).FullName,
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<DefaultControllerAttribute>()
                            ? classDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            return defaultControllers!;
        }
    }
}
