using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class SimpleBaseTypeSyntaxExtensions
    {
        public static string GetNamespace(this SimpleBaseTypeSyntax classDeclaration)
        {
            // Get the root of the syntax tree
            var root = classDeclaration.SyntaxTree.GetRoot();

            // Traverse up to find the NamespaceDeclarationSyntax or CompilationUnitSyntax
            var namespaceDeclaration = classDeclaration
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();
            if (namespaceDeclaration != null)
            {
                return namespaceDeclaration.Name.ToString();
            }

            // If not found, check the compilation unit (the top-level container)
            var compilationUnit = classDeclaration
                .Ancestors()
                .OfType<CompilationUnitSyntax>()
                .FirstOrDefault();
            if (compilationUnit != null)
            {
                return string.Empty; // No namespace, top-level code
            }

            // Return null or empty if not found
            return null;
        }
    }
}
