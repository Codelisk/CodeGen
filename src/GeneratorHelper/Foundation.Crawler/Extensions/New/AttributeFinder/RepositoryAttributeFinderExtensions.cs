﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New.AttributeFinder
{
    public static class RepositoryAttributeFinderExtensions
    {
        public static ClassDeclarationSyntax Repository(
            this RecordDeclarationSyntax dto,
            IEnumerable<ClassDeclarationSyntax> repos
        )
        {
            return dto.TenantOrDefault<DefaultRepositoryAttribute>(repos);
        }

        public static IncrementalValueProvider<
            ImmutableArray<ClassDeclarationSyntax>
        > DefaultRepositories(this IncrementalGeneratorInitializationContext context)
        {
            var defaultControllers = context
                .SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(DefaultRepositoryAttribute).FullName,
                    static (n, _) => n is ClassDeclarationSyntax,
                    static (context, cancellationToken) =>
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)
                            context.TargetNode;

                        return classDeclarationSyntax.HasAttribute<DefaultRepositoryAttribute>()
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
