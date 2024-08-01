using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New
{
    public static class DtoExtensions
    {
        private static string GetNamespace(SyntaxNode node)
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

        public static string GetFullModelName(this ClassDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.Replace("Dto", "Full");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }

        public static string GetEntityName(this ClassDeclarationSyntax dto, bool plural = false)
        {
            var name = dto.Identifier.Text.Replace("Dto", "Entity");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
