using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class IAssemblySymbolExtensions
    {/// <summary>
     /// Get all type symbols of an assembly
     /// Iterates through all members of the global namespace in the assembly
     /// When the member is another namespace it gets added to the stack
     /// When the member is a NamedTypeSymbol it gets added to the result
     /// </summary>
        public static IEnumerable<INamedTypeSymbol> GetAllTypeSymbols(this IAssemblySymbol assembly)
        {
            var symbols = new Stack<INamespaceSymbol>();
            symbols.Push(assembly.GlobalNamespace);

            while (symbols.Count > 0)
            {
                var @namespace = symbols.Pop();

                foreach (var symbolMember in @namespace.GetMembers())
                {
                    if (symbolMember is INamespaceSymbol memberAsNamespace)
                    {
                        symbols.Push(memberAsNamespace);
                    }
                    else if (symbolMember is INamedTypeSymbol symbol)
                    {
                        yield return symbol;
                    }
                }
            }
        }
    }
}
