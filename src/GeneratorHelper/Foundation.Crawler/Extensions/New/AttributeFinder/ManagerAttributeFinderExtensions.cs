﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New.AttributeFinder
{
    public static class ManagerAttributeFinderExtensions
    {
        public static IncrementalValueProvider<
            ImmutableArray<ClassDeclarationSyntax>
        > DefaultManagers(this IncrementalGeneratorInitializationContext context)
        {
            var defaultControllers = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DefaultManagerAttribute).FullName,
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<DefaultManagerAttribute>()
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