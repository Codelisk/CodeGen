using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class RecordDeclarationSyntaxExtensions
    {
        public static RecordDeclarationSyntax Construct(
            this RecordDeclarationSyntax symbol,
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
            this RecordDeclarationSyntax RecordDeclarationSyntax
        )
        {
            // Check if the type declaration has the BaseContext attribute
            foreach (var attributeList in RecordDeclarationSyntax.AttributeLists)
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

        public static AttributeSyntax GetAttribute<TAttribute>(this RecordDeclarationSyntax record)
            where TAttribute : Attribute
        {
            var attributeName = typeof(TAttribute).Name;

            // Iterate over all attribute lists and their attributes in the record
            return record
                .AttributeLists.SelectMany(attrList => attrList.Attributes)
                .LastOrDefault(attr =>
                    attr.Name.ToString().Equals(attributeName)
                    || attr.Name.ToString().Equals(attributeName.Replace("Attribute", ""))
                );
        }

        public static string GetName(this RecordDeclarationSyntax RecordDeclaration)
        {
            return RecordDeclaration.Identifier.Text;
        }

        public static string GetNamespace(this RecordDeclarationSyntax RecordDeclaration)
        {
            // Get the root of the syntax tree
            var root = RecordDeclaration.SyntaxTree.GetRoot();

            // Traverse up to find the NamespaceDeclarationSyntax or CompilationUnitSyntax
            var namespaceDeclaration = RecordDeclaration
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();
            if (namespaceDeclaration != null)
            {
                return namespaceDeclaration.Name.ToString();
            }

            // If not found, check the compilation unit (the top-level container)
            var compilationUnit = RecordDeclaration
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

        public static IEnumerable<PropertyDeclarationSyntax> GetProperties(
            this RecordDeclarationSyntax classSyntax
        )
        {
            return classSyntax.Members.OfType<PropertyDeclarationSyntax>();
        }

        // Hilfsmethode, um eine Klasse im Syntaxbaum zu finden
        private static ClassDeclarationSyntax FindClassInSyntaxTree(
            string className,
            BaseTypeSyntax rootSyntax
        )
        {
            // Suche im Syntaxbaum nach der Klasse mit dem angegebenen Namen
            return rootSyntax
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text.Equals(className, StringComparison.Ordinal));
        }

        public static IEnumerable<IPropertySymbol>? GetAllPropertiesWithBaseClass(
            this RecordDeclarationSyntax RecordDeclaration,
            SemanticModel semanticModel,
            bool onlyPublic
        )
        {
            var allProperties = new List<IPropertySymbol>();

            var classSymbol =
                semanticModel.GetDeclaredSymbol(RecordDeclaration) as INamedTypeSymbol;
            if (classSymbol == null)
                return null;

            allProperties = classSymbol.GetAllProperties(true);
            if (onlyPublic)
            {
                return allProperties.Where(p => p.DeclaredAccessibility == Accessibility.Public);
            }

            return allProperties;
        }

        public static List<PropertyDeclarationSyntax> GetAllProperties(
            this RecordDeclarationSyntax RecordDeclaration,
            bool onlyPublic
        )
        {
            var properties = new List<PropertyDeclarationSyntax>();

            var classProperties = RecordDeclaration.Members.OfType<PropertyDeclarationSyntax>();
            if (onlyPublic)
            {
                classProperties = classProperties.Where(p =>
                    p.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                );
            }
            properties.AddRange(classProperties);

            return properties.ToList();
        }

        #region Methods

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(
            this RecordDeclarationSyntax classSyntax
        )
        {
            return classSyntax.Members.OfType<MethodDeclarationSyntax>();
        }

        public static IEnumerable<MethodDeclarationSyntax> GetMethodsWithAttributes(
            this RecordDeclarationSyntax classSyntax,
            string attributeFullName
        )
        {
            return classSyntax
                .GetMethods()
                .Where(method =>
                    method
                        .AttributeLists.SelectMany(attrList => attrList.Attributes)
                        .Any(attr => attr.Name.ToString() == attributeFullName)
                );
        }

        static void ExtractBaseMethods(
            BaseListSyntax baseList,
            IEnumerable<RecordDeclarationSyntax> baseClasses, // Hier verwenden wir ClassDeclarationSyntax
            List<MethodDeclarationSyntax> methods // Liste, um die Methoden zu speichern
        )
        {
            if (baseList == null)
                return; // Keine Basisklasse vorhanden

            foreach (var baseTypeSyntax in baseList.Types)
            {
                // Hole den Namen der Basisklasse
                var baseTypeName = baseTypeSyntax.Type.GetName();

                if (!string.IsNullOrEmpty(baseTypeName))
                {
                    // Suche nach der Basisklasse in den bekannten baseClasses
                    var baseClass = baseClasses.FirstOrDefault(x =>
                        x.Identifier.Text == baseTypeName
                    );

                    if (baseClass != null)
                    {
                        // Füge die Methoden der Basisklasse hinzu
                        methods.AddRange(baseClass.GetMethods());

                        // Rekursiv weitermachen, falls diese Basisklasse ebenfalls eine Basisklasse hat
                        ExtractBaseMethods(baseClass.BaseList, baseClasses, methods);
                    }
                }
            }
        }

        public static string GetIdPropertyMethodeName(
            this RecordDeclarationSyntax dto,
            IEnumerable<RecordDeclarationSyntax> baseDtos
        )
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            methods.AddRange(dto.GetMethods());
            ExtractBaseMethods(dto.BaseList, baseDtos, methods);

            return methods.First(x => x.HasAttribute(nameof(GetIdAttribute))).GetName();
        }
        #endregion
    }
}
