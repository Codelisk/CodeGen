using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Database;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New.AttributeFinder
{
    public static class DbContextAttributeFinderExtensions
    {
        public static IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax>> DbContexts(
            this IncrementalGeneratorInitializationContext context
        )
        {
            var baseContext = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(BaseContextAttribute).FullName,
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<BaseContextAttribute>()
                            ? classDeclarationSyntax
                            : null;
                    }
                )
                .Where(static typeDeclaration => typeDeclaration is not null)
                .Collect();

            return baseContext!;
        }
    }
}
