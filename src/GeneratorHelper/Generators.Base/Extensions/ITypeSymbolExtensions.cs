using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class ITypeSymbolExtensions
    {
        public static bool IsListType(this ITypeSymbol typeSymbol)
        {
            // Get all the base types of the type
            var baseTypes = new HashSet<ITypeSymbol>();
            typeSymbol.GetAllBaseTypes(baseTypes);

            // Check if the type implements any of the list-like interfaces
            return baseTypes.Any(IsListLikeInterface);
        }

        private static void GetAllBaseTypes(this ITypeSymbol typeSymbol, HashSet<ITypeSymbol> baseTypes)
        {
            if (typeSymbol == null || baseTypes.Contains(typeSymbol))
            {
                return;
            }

            baseTypes.Add(typeSymbol);
            GetAllBaseTypes(typeSymbol.BaseType, baseTypes);

            foreach (var @interface in typeSymbol.Interfaces)
            {
                GetAllBaseTypes(@interface, baseTypes);
            }
        }

        private static bool IsListLikeInterface(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol.ToDisplayString().StartsWith("System.Collections."))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if a given type is IEnumerable<> (or inherits from it)
        public static bool IsEnumerableType(this ITypeSymbol typeSymbol, Compilation compilation)
        {
            // Get the IEnumerable<> interface
            var enumerableInterface = compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");

            // Check if the type implements the IEnumerable<> interface or inherits from it
            return typeSymbol.AllInterfaces.Any(interfaceType =>
                interfaceType.OriginalDefinition.Equals(enumerableInterface));
        }
    }
}
