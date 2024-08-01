using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static bool HasAttribute<TAttribute>(
            this ClassDeclarationSyntax classDeclarationSyntax
        )
        {
            // Check if the type declaration has the BaseContext attribute
            foreach (var attributeList in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    var sdf = attribute.Name.ToFullString();
                    if (attribute.Name.ToString() == typeof(TAttribute).RealAttributeName())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GetNamespace(this ClassDeclarationSyntax classDeclaration)
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

        public static List<PropertyDeclarationSyntax> GetAllProperties(
            this ClassDeclarationSyntax classDeclaration,
            bool withBaseTypes,
            bool onlyPublic
        )
        {
            var properties = new List<PropertyDeclarationSyntax>();
            var currentClass = classDeclaration;

            while (currentClass != null)
            {
                var classProperties = currentClass.Members.OfType<PropertyDeclarationSyntax>();
                if (onlyPublic)
                {
                    classProperties = classProperties.Where(p =>
                        p.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                    );
                }
                properties.AddRange(classProperties);

                if (!withBaseTypes)
                    break;
                currentClass = currentClass.GetBaseClass();
            }

            return properties.ToList();
        }

        private static ClassDeclarationSyntax GetBaseClass(
            this ClassDeclarationSyntax classDeclaration
        )
        {
            var baseType = classDeclaration.BaseList?.Types.FirstOrDefault();
            if (baseType == null)
                return null;

            var root = classDeclaration.SyntaxTree.GetRoot();
            return root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text == baseType.Type.ToString());
        }
    }
}
