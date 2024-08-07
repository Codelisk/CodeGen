using System.Reflection;
using System.Text;
using CodeGenHelpers.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions
{
    public static class INamedTypeSymbolExtensions
    {
        public static string GetClassSourceCode(
            this INamedTypeSymbol classSymbol,
            Compilation compilation
        )
        {
            // Find the syntax reference for the class
            var syntaxReference = classSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxReference == null)
            {
                return string.Empty;
            }

            // Get the syntax node for the class
            var syntaxNode = syntaxReference.GetSyntax();
            if (syntaxNode == null)
            {
                return string.Empty;
            }

            // Get the syntax tree and root node
            var syntaxTree = syntaxNode.SyntaxTree;
            var root = syntaxTree.GetRoot();

            // Get the using directives
            var usings = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
            var usingDirectives = string.Join(
                "\n",
                usings.Select(u => u.NormalizeWhitespace().ToFullString())
            );

            // Normalize whitespace and convert class to full string
            var classDeclaration = syntaxNode as ClassDeclarationSyntax;
            var classCode = classDeclaration?.NormalizeWhitespace().ToFullString() ?? string.Empty;

            // Combine using directives and class code
            var sourceBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(usingDirectives))
            {
                sourceBuilder.AppendLine(usingDirectives);
                sourceBuilder.AppendLine();
            }
            sourceBuilder.AppendLine(classCode);

            return sourceBuilder.ToString();
        }

        private static string GetAssemblyQualifiedName(this INamedTypeSymbol namedTypeSymbol)
        {
            var namespaceParts = namedTypeSymbol.ContainingNamespace.ToDisplayString().Split('.');
            var typeName = namedTypeSymbol.Name;
            var assemblyName = namedTypeSymbol.ContainingAssembly.Name;

            return $"{string.Join(".", namespaceParts)}.{typeName}, {assemblyName}";
        }

        public static IPropertySymbol GetFirstPropertyByName(
            this INamedTypeSymbol classObject,
            string name,
            string nameSpace = null
        )
        {
            var properties = classObject.GetMembers().OfType<IPropertySymbol>();

            var result = properties.FirstOrDefault(x =>
                x.Name.Equals(name)
                && (
                    nameSpace is null
                    || x.ContainingNamespace.GetGloballyQualifiedTypeName().Equals(nameSpace)
                )
            );

            if (result is not null)
            {
                return result;
            }

            return classObject.GetFirstPropertyByNameInBaseClasses(name, nameSpace);
        }

        private static IPropertySymbol GetFirstPropertyByNameInBaseClasses(
            this INamedTypeSymbol classObject,
            string name,
            string nameSpace
        )
        {
            // Recursively add properties from base classes
            INamedTypeSymbol baseType = classObject.BaseType;
            while (baseType != null && baseType.SpecialType != SpecialType.System_Object)
            {
                var result = baseType
                    .GetMembers()
                    .OfType<IPropertySymbol>()
                    .FirstOrDefault(x =>
                        x.Name.Equals(name)
                        && (
                            nameSpace is null
                            || x.ContainingNamespace.GetGloballyQualifiedTypeName()
                                .Equals(nameSpace)
                        )
                    );
                if (result is not null)
                {
                    return result;
                }
                baseType = baseType.BaseType;
            }

            return null;
        }

        public static IFieldSymbol GetFieldWithAttribute(
            this INamedTypeSymbol classObject,
            string attributeName
        )
        {
            return classObject.GetAllFields().Where(x => x.HasAttribute(attributeName)).First();
        }

        public static List<IFieldSymbol> GetFieldsWithConstructedFromType(
            this INamedTypeSymbol classObject,
            INamedTypeSymbol type
        )
        {
            return classObject
                .GetAllFields()
                .Where(x =>
                {
                    if (
                        SymbolEqualityComparer.Default.Equals(
                            (x.Type as INamedTypeSymbol).ConstructedFrom,
                            type.ConstructedFrom
                        )
                    )
                    {
                        return true;
                    }

                    foreach (var i in type.ConstructedFrom.AllInterfaces)
                    {
                        if (
                            SymbolEqualityComparer.Default.Equals(
                                (x.Type as INamedTypeSymbol).ConstructedFrom,
                                i.ConstructedFrom
                            )
                        )
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .ToList();
        }

        public static IPropertySymbol? GetPropertyWithAttribute(
            this INamedTypeSymbol classObject,
            string attributeName
        )
        {
            return classObject
                .GetAllProperties()
                .Where(x => x.HasAttribute(attributeName))
                .FirstOrDefault();
        }

        public static List<IPropertySymbol> GetPropertiesWithType(
            this INamedTypeSymbol classObject,
            ITypeSymbol type
        )
        {
            return classObject
                .GetAllProperties()
                .Where(x => SymbolEqualityComparer.Default.Equals(x.Type, type))
                .ToList();
        }

        public static List<IPropertySymbol> GetAllProperties(
            this INamedTypeSymbol classObject,
            bool withBaseType = false
        )
        {
            var result = classObject.GetMembers().OfType<IPropertySymbol>().ToList();

            if (withBaseType)
            {
                classObject = classObject.BaseType;
                while (
                    classObject is not null && classObject.SpecialType != SpecialType.System_Object
                )
                {
                    result.AddRange(classObject.GetMembers().OfType<IPropertySymbol>().ToList());

                    classObject = classObject.BaseType;
                }
            }

            return result;
        }

        public static List<IFieldSymbol> GetAllFields(this INamedTypeSymbol classObject)
        {
            var result = classObject.GetMembers().OfType<IFieldSymbol>().ToList();
            return result;
        }

        public static AttributeData GetAttribute<TAttribut>(this INamedTypeSymbol classObject)
            where TAttribut : Attribute
        {
            return classObject
                .GetAttributes()
                .Where(x => x.GetAttributeName().Equals(typeof(TAttribut).Name))
                .Last();
        }

        public static IEnumerable<ISymbol> GetMembersWithAttribute(
            this INamedTypeSymbol classObject,
            string fullAttributeName
        )
        {
            var allMembers = classObject.GetMembers();

            return allMembers.Where(x => x.HasAttribute(fullAttributeName)).ToList();
        }

        public static bool IsDerivedFromClass(
            this INamedTypeSymbol classSymbol,
            INamedTypeSymbol baseControllerType
        )
        {
            if (classSymbol != null)
            {
                if (classSymbol.Equals(baseControllerType))
                {
                    return true;
                }

                foreach (var interfaceType in classSymbol.Interfaces)
                {
                    if (interfaceType.Equals(baseControllerType))
                    {
                        return true;
                    }
                }

                var baseType = classSymbol.BaseType;
                if (baseType != null)
                {
                    return IsDerivedFromClass(baseType, baseControllerType);
                }
            }

            return false;
        }

        public static IEnumerable<IMethodSymbol> GetMethods(this INamedTypeSymbol symbol)
        {
            return symbol.GetMembers().OfType<IMethodSymbol>();
        }

        public static List<IMethodSymbol> GetMethodsIncludingBaseTypes(this INamedTypeSymbol symbol)
        {
            List<IMethodSymbol> methods = new List<IMethodSymbol>();
            while (symbol is not null)
            {
                methods.AddRange(symbol.GetMethods());
                symbol = symbol.BaseType;
            }

            return methods;
        }

        public static IEnumerable<IMethodSymbol> GetMethodsWithAttributesIncludingBaseTypes(
            this INamedTypeSymbol symbol,
            string attributeFullName
        )
        {
            return GetMethodsWithAttributesIncludingBaseTypes(symbol)
                .Where(x =>
                    x.GetAttributes()
                        .Any(attr =>
                        {
                            return attr.GetAttributeName().Equals(attributeFullName);
                        })
                );
        }

        public static IEnumerable<IMethodSymbol> GetMethodsWithAttributesIncludingBaseTypes(
            this INamedTypeSymbol symbol
        )
        {
            return symbol.GetMethodsIncludingBaseTypes().Where(x => x.GetAllAttributes().Any());
        }

        public static IEnumerable<IMethodSymbol> GetMethodsWithAttributes(
            this INamedTypeSymbol symbol
        )
        {
            return symbol.GetMethods().Where(x => x.GetAllAttributes().Any());
        }

        public static string GetReturnTypeName(this ITypeSymbol returnType)
        {
            if (returnType is INamedTypeSymbol n && n.IsWellKnownSystemType())
            {
                return returnType.ToDisplayString();
            }
            else
            {
                return returnType.Name;
            }
        }

        public static IEnumerable<IMethodSymbol> GetMethodsWithAttribute(
            this INamedTypeSymbol symbol,
            string attributeFullName
        )
        {
            if (symbol == null || string.IsNullOrEmpty(attributeFullName))
                throw new ArgumentNullException();

            var result = symbol
                .GetMethods()
                .Where(method =>
                    method
                        .GetAttributes()
                        .Any(attr =>
                        {
                            return attr.GetAttributeName().Equals(attributeFullName);
                        })
                );

            if (!result.Any() && symbol.BaseType is not null)
            {
                return symbol
                    .BaseType.GetMethods()
                    .Where(method =>
                        method
                            .GetAttributes()
                            .Any(attr =>
                            {
                                return attr.GetAttributeName().Equals(attributeFullName);
                            })
                    );
            }

            return result;
        } // Check if a given TypeSymbol is a well-known type from the System namespace or other system namespaces

        public static bool IsWellKnownSystemType(this INamedTypeSymbol typeSymbol)
        {
            if (typeSymbol is null)
            {
                return false;
            }

            var systemNamespace = "System";
            return typeSymbol.ContainingNamespace?.ToDisplayString() == systemNamespace
                || typeSymbol
                    .ContainingNamespace?.ToDisplayString()
                    .StartsWith(systemNamespace + ".") == true
                || typeSymbol.ContainingNamespace?.IsGlobalNamespace == true;
        }

        public static IEnumerable<INamedTypeSymbol> GetDistinctNamedTypeSymbols(
            this IEnumerable<INamedTypeSymbol> namedTypeSymbols
        )
        {
            var processedSymbols = new HashSet<string>();
            var distinctSymbols = new List<INamedTypeSymbol>();

            foreach (var symbol in namedTypeSymbols)
            {
                var dtoClass = symbol.IsGenericType
                    ? symbol.TypeArguments[0] as INamedTypeSymbol
                    : symbol;

                if (!dtoClass.IsGenericType && !dtoClass.IsAbstract)
                {
                    if (!processedSymbols.Contains(dtoClass.GetFullName()))
                    {
                        distinctSymbols.Add(dtoClass);
                        processedSymbols.Add(dtoClass.GetFullName());
                    }
                }
            }

            return distinctSymbols;
        }

        // Get the element type of the IEnumerable<> (or inherited) type
        public static ITypeSymbol GetEnumerableElementType(this INamedTypeSymbol typeSymbol)
        {
            return typeSymbol.TypeArguments.FirstOrDefault();
        }
    }
}
