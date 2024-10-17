using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class SyntaxNodeExtensions
    {
        public static string GetNamespace(this SyntaxNode node)
        {
            var nameSpace = string.Empty;
            for (SyntaxNode? current = node; current != null; current = current.Parent)
            {
                if (current is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    nameSpace = namespaceDeclaration.Name.ToString() + "." + nameSpace;
                }
                else if (current is FileScopedNamespaceDeclarationSyntax fileScopedNamespace)
                {
                    nameSpace = fileScopedNamespace.Name.ToString() + "." + nameSpace;
                }
                else if (current is CompilationUnitSyntax)
                {
                    // Reached the root, stop traversing
                    break;
                }
            }
            return nameSpace.TrimEnd('.');
        }
    }
}
