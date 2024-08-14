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
        public static ClassDeclarationSyntax Construct(
            this ClassDeclarationSyntax symbol,
            params string[] typeArguments
        )
        {
            // Erstelle eine Liste der TypeArgumentSyntax-Knoten basierend auf den übergebenen Strings
            var typeArgumentList = SyntaxFactory.TypeArgumentList(
                SyntaxFactory.SeparatedList<TypeSyntax>(
                    typeArguments.Select(arg => SyntaxFactory.ParseTypeName(arg))
                )
            );

            // Erstelle eine neue BaseList, wenn Typargumente vorhanden sind
            var newBaseList =
                symbol.BaseList != null
                    ? symbol.BaseList.WithTypes(
                        SyntaxFactory.SeparatedList<BaseTypeSyntax>(
                            symbol.BaseList.Types.Select(bt =>
                                bt.WithType(
                                    SyntaxFactory.GenericName(
                                        ((IdentifierNameSyntax)bt.Type).Identifier,
                                        typeArgumentList
                                    )
                                )
                            )
                        )
                    )
                    : null;

            // Rückgabe einer neuen Klasse mit den modifizierten Basisklassen
            return symbol.WithBaseList(newBaseList);
        }

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

        public static string GetName(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Identifier.Text;
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

        public static IEnumerable<IPropertySymbol>? GetAllPropertiesWithBaseClass(
            this ClassDeclarationSyntax classDeclaration,
            SemanticModel semanticModel,
            bool onlyPublic
        )
        {
            var allProperties = new List<IPropertySymbol>();

            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
            if (classSymbol == null)
                return null;

            allProperties = classSymbol.GetAllProperties(true);
            if (onlyPublic)
            {
                return allProperties.Where(p => p.DeclaredAccessibility == Accessibility.Public);
            }

            return allProperties;
        }

        private static IEnumerable<INamedTypeSymbol> GetBaseTypes(INamedTypeSymbol type)
        {
            var baseTypes = new List<INamedTypeSymbol>();
            var currentType = type.BaseType;

            while (currentType != null)
            {
                baseTypes.Add(currentType);
                currentType = currentType.BaseType;
            }

            return baseTypes;
        }

        public static List<PropertyDeclarationSyntax> GetAllProperties(
            this ClassDeclarationSyntax classDeclaration,
            bool onlyPublic
        )
        {
            var properties = new List<PropertyDeclarationSyntax>();

            var classProperties = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();
            if (onlyPublic)
            {
                classProperties = classProperties.Where(p =>
                    p.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                );
            }
            properties.AddRange(classProperties);

            return properties.ToList();
        }
    }
}
