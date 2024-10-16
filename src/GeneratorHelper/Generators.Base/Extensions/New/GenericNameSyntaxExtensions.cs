using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class GenericNameSyntaxExtensions
    {
        public static string GetNamespace(this GenericNameSyntax classDeclaration)
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

        public static string GetFullTypeName(this GenericNameSyntax c, bool includeNamespace = true)
        {
            // Start with the class name (Identifier)
            var fullTypeName = c.Identifier.Text;

            // Check if the class has type parameters (generics)
            if (c.TypeArgumentList != null && c.TypeArgumentList.Arguments.Any())
            {
                var typeParameters = string.Join(
                    ", ",
                    c.TypeArgumentList.Arguments.Select(p => p.GetName())
                );
                fullTypeName += $"<{typeParameters}>";
            }

            // Traverse any containing types (to handle nested types)
            var parent = c.Parent;
            while (parent is TypeDeclarationSyntax typeDeclaration)
            {
                var parentTypeName = typeDeclaration.Identifier.Text;

                // Handle generics for the containing type
                if (
                    typeDeclaration is ClassDeclarationSyntax parentClass
                    && parentClass.TypeParameterList != null
                    && parentClass.TypeParameterList.Parameters.Any()
                )
                {
                    var parentTypeParameters = string.Join(
                        ", ",
                        parentClass.TypeParameterList.Parameters.Select(p => p.Identifier.Text)
                    );
                    parentTypeName += $"<{parentTypeParameters}>";
                }

                fullTypeName = $"{parentTypeName}.{fullTypeName}";
                parent = parent.Parent;
            }

            // Get the namespace, if available
            var namespaceName = parent.GetNamespace();
            if (includeNamespace && !string.IsNullOrEmpty(namespaceName))
            {
                fullTypeName = $"{namespaceName}.{fullTypeName}";
            }

            return fullTypeName;
        }

        public static string GetFullName(this GenericNameSyntax genericNameSyntax)
        {
            // Get the name of the generic type (e.g., List or Dictionary)
            var genericTypeName = genericNameSyntax.Identifier.Text;

            // Get the type arguments (e.g., T, K, V)
            var typeArguments = genericNameSyntax
                .TypeArgumentList.Arguments.Select(arg => arg.ToString()) // Get type arguments as string
                .ToArray();

            // Construct the full name with type arguments
            return $"{genericTypeName}<{string.Join(", ", typeArguments)}>";
        }
    }
}
