using CodeGenHelpers.Internals;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class INamedTypeSymbolExtensions
    {
        public static IFieldSymbol GetFieldWithAttribute(this INamedTypeSymbol classObject, string attributeName)
        {
            return classObject.GetAllFields().Where(x => x.HasAttribute(attributeName)).First();
        }
        public static List<IFieldSymbol> GetFieldsWithConstructedFromType(this INamedTypeSymbol classObject, INamedTypeSymbol type)
        {
            return classObject.GetAllFields().Where(x => SymbolEqualityComparer.Default.Equals((x.Type as INamedTypeSymbol).ConstructedFrom, type.ConstructedFrom)).ToList();
        }
        public static IPropertySymbol GetPropertyWithAttribute(this INamedTypeSymbol classObject, string attributeName)
        {
            return classObject.GetAllProperties().Where(x => x.HasAttribute(attributeName)).First();
        }
        public static List<IPropertySymbol> GetPropertiesWithType(this INamedTypeSymbol classObject, ITypeSymbol type)
        {
            return classObject.GetAllProperties().Where(x => SymbolEqualityComparer.Default.Equals(x.Type, type)).ToList();
        }
        public static List<IPropertySymbol> GetAllProperties(this INamedTypeSymbol classObject)
        {
            return classObject.GetMembers()
                    .OfType<IPropertySymbol>()
                    .ToList();
        }
        public static List<IFieldSymbol> GetAllFields(this INamedTypeSymbol classObject)
        {
            var result = classObject.GetMembers()
                    .OfType<IFieldSymbol>()
                    .ToList();
            return result;
        }
        public static IEnumerable<ISymbol> GetMembersWithAttribute(this INamedTypeSymbol classObject, string fullAttributeName)
        {
            var allMembers = classObject.GetMembers();


            return allMembers.Where(x => x.HasAttribute(fullAttributeName)).ToList();
        }
        public static bool IsDerivedFromClass(this INamedTypeSymbol classSymbol, INamedTypeSymbol baseControllerType)
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
            return symbol.GetMembers()
                         .OfType<IMethodSymbol>();
        }
        public static string GetReturnTypeName(this ITypeSymbol returnType)
        {
            if(returnType is INamedTypeSymbol n && n.IsWellKnownSystemType())
            {
                return returnType.ToDisplayString();
            }
            else
            {
                return returnType.Name;
            }
        }
        public static IEnumerable<IMethodSymbol> GetMethodsWithAttribute(this INamedTypeSymbol symbol, string attributeFullName)
        {
            if (symbol == null || string.IsNullOrEmpty(attributeFullName))
                throw new ArgumentNullException();

            return symbol.GetMethods()
                         .Where(method => method.GetAttributes().Any(attr => 
                         {
                             return attr.GetAttributeName().Equals(attributeFullName);
                             }));
        }// Check if a given TypeSymbol is a well-known type from the System namespace or other system namespaces
        public static bool IsWellKnownSystemType(this INamedTypeSymbol typeSymbol)
        {
            if(typeSymbol is null)
            {
                return false;
            }

            var systemNamespace = "System";
            return typeSymbol.ContainingNamespace?.ToDisplayString() == systemNamespace
                   || typeSymbol.ContainingNamespace?.ToDisplayString().StartsWith(systemNamespace + ".") == true
                   || typeSymbol.ContainingNamespace?.IsGlobalNamespace == true;
        }

        public static IEnumerable<INamedTypeSymbol> GetDistinctNamedTypeSymbols(this IEnumerable<INamedTypeSymbol> namedTypeSymbols)
        {
            var processedSymbols = new HashSet<string>();
            var distinctSymbols = new List<INamedTypeSymbol>();

            foreach (var symbol in namedTypeSymbols)
            {
                var dtoClass = symbol.IsGenericType ? symbol.TypeArguments[0] as INamedTypeSymbol : symbol;

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
