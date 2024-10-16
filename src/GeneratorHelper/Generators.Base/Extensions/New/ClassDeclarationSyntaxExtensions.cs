using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public static List<FieldDeclarationSyntax> GetFieldsWithConstructedFromType(
            this ClassDeclarationSyntax classDeclaration,
            string typeName
        )
        {
            var fields = new List<FieldDeclarationSyntax>();

            // Iterate over all field declarations in the class
            foreach (var member in classDeclaration.GetAllFields())
            {
                // Get the type of the field (syntax-based)
                var fieldType = member.Declaration.Type;

                // Check if the field type matches the provided typeName
                if (
                    fieldType is IdentifierNameSyntax identifierNameSyntax
                    && identifierNameSyntax.Identifier.Text == typeName
                )
                {
                    fields.Add(member);
                }
                else if (fieldType is GenericNameSyntax genericNameSyntax)
                {
                    var fullTypeName = genericNameSyntax.GetFullTypeName(false);
                    // In case of generic types, we check the base generic type name
                    if (fullTypeName.Equals(typeName))
                    {
                        fields.Add(member);
                    }
                }
            }

            return fields;
        }

        public static List<FieldDeclarationSyntax> GetAllFields(
            this ClassDeclarationSyntax classObject
        )
        {
            var result = classObject.Members.OfType<FieldDeclarationSyntax>().ToList();
            return result;
        }

        public static ClassDeclarationSyntax Construct(
            this ClassDeclarationSyntax classDeclaration,
            RecordDeclarationSyntax dto
        )
        {
            // Check if the dto has a TypeParameterList
            if (dto.TypeParameterList == null)
            {
                // If no type parameters, return the original class declaration
                return classDeclaration;
            }

            // Create a new ClassDeclarationSyntax with the same name and modifiers
            var typeParameters = dto
                .TypeParameterList.Parameters.Select(tp =>
                    SyntaxFactory.TypeParameter(tp.Identifier.Text)
                )
                .ToArray();

            // Create a new TypeParameterListSyntax
            var newTypeParameterList = SyntaxFactory.TypeParameterList(
                SyntaxFactory.SeparatedList(typeParameters)
            );

            // Create the new ClassDeclarationSyntax with the new type parameters
            return classDeclaration.WithTypeParameterList(newTypeParameterList);
        }

        public static AttributeSyntax GetAttribute<TAttribute>(this ClassDeclarationSyntax c)
            where TAttribute : Attribute
        {
            var attributeName = typeof(TAttribute).Name;

            // Iterate over all attribute lists and their attributes in the class
            return c
                .AttributeLists.SelectMany(attrList => attrList.Attributes)
                .LastOrDefault(attr =>
                    attr.Name.ToString().Equals(attributeName)
                    || attr.Name.ToString().Equals(attributeName.Replace("Attribute", ""))
                );
        }

        public static string GetBaseClassName(this ClassDeclarationSyntax classDeclaration)
        {
            // Prüfen, ob eine Basisklasse vorhanden ist
            var baseTypeSyntax = classDeclaration.BaseList?.Types.FirstOrDefault();

            // Wenn keine Basisklasse vorhanden ist, gib einen leeren String zurück
            if (baseTypeSyntax == null)
                return string.Empty;

            // Extrahiere den Namen der Basisklasse als String
            return baseTypeSyntax.Type.ToString();
        }

        public static string GetFirstInterfaceFullTypeName(
            this ClassDeclarationSyntax classDeclaration,
            bool includeNamespace = true
        )
        {
            // Prüfen, ob eine Basisklasse vorhanden ist
            var baseTypeSyntax = classDeclaration
                .BaseList?.Types.OfType<SimpleBaseTypeSyntax>()
                .First(x => x.Type is GenericNameSyntax);

            // Wenn keine Basisklasse vorhanden ist, gib einen leeren String zurück
            if (baseTypeSyntax == null)
                return string.Empty;

            var result = baseTypeSyntax.ToString();

            // Get the namespace, if available
            var namespaceName = baseTypeSyntax.GetNamespace();
            if (includeNamespace && !string.IsNullOrEmpty(namespaceName))
            {
                result = $"{namespaceName}.{result}";
            }

            return result;
        }

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

        #region Methods

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(
            this ClassDeclarationSyntax classSyntax
        )
        {
            return classSyntax.Members.OfType<MethodDeclarationSyntax>();
        }

        public static IEnumerable<MethodDeclarationSyntax> GetMethodsWithBaseClasses(
            this ClassDeclarationSyntax classSyntax,
            ImmutableArray<ClassDeclarationSyntax> baseClasses
        )
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            methods.AddRange(classSyntax.GetMethods());
            ExtractBaseMethods(classSyntax.BaseList, baseClasses, methods);

            return methods;
        }

        public static IEnumerable<MethodDeclarationSyntax> GetMethodsWithAttributes<TAttribute>(
            this ClassDeclarationSyntax classSyntax,
            ImmutableArray<ClassDeclarationSyntax> baseClasses
        )
            where TAttribute : Attribute
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            methods.AddRange(classSyntax.GetMethods());
            ExtractBaseMethods(classSyntax.BaseList, baseClasses, methods);

            string attributeName = typeof(TAttribute).RealAttributeName();

            return methods.Where(method =>
                method
                    .AttributeLists.SelectMany(attrList => attrList.Attributes)
                    .Any(attr => attr.Name.ToString() == attributeName)
            );
        }

        static void ExtractBaseMethods(
            BaseListSyntax baseList,
            IEnumerable<ClassDeclarationSyntax> baseClasses, // Hier verwenden wir ClassDeclarationSyntax
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
        #endregion
    }
}
