using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class GeneratorExecutionContextExtensions
    {
        public static INamedTypeSymbol GetClassByName(this GeneratorExecutionContext context, string name, string assemblyName)
        {
            return context.GetAllClasses(assemblyName).Single(x => x.Name.Equals(name));
        }
        public static INamedTypeSymbol GetClass(this GeneratorExecutionContext context, Type t, string assemblyName)
        {
            return context.GetAllClasses(assemblyName).Single(x => x.Name == t.Name);
        }
        public static INamedTypeSymbol GetClass<T>(this GeneratorExecutionContext context, string assemblyName)
        {
            return context.GetAllClasses(assemblyName).Single(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Equals(typeof(T).Name));
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass<T>(this GeneratorExecutionContext context, string assemblyName = "")
        {
            var name = typeof(T).Name;
            return context.GetAllClasses(assemblyName).Where(x => x.BaseType?.Name is not null && x.BaseType.Name.Equals(name));
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass(this GeneratorExecutionContext context, INamedTypeSymbol baseClass, string assemblyName = "")
        {
            return context.GetAllClasses(assemblyName).Where(x => SymbolEqualityComparer.Default.Equals(x.BaseType, baseClass));
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttribute(this GeneratorExecutionContext context, string fullAttributeName, string assemblyName = "")
        {
            return context.GetAllClasses(assemblyName).Where(x => x.HasAttributeWithoutBaseClass(fullAttributeName));
        }
        public static IEnumerable<INamedTypeSymbol> GetAllClasses(this GeneratorExecutionContext context, string assemblyName)
        {
            var result = new List<INamedTypeSymbol>();

            foreach (var assembly in context.GetAssembilesWithPrefix(assemblyName))
            {
                result.AddRange(assembly.GetAllTypeSymbols());
            }

            return result;
        }
        public static IEnumerable<IAssemblySymbol> GetAssembilesWithPrefix(this GeneratorExecutionContext context, string prefix)
        {
            yield return context.Compilation.Assembly;

            foreach (var reference in context.Compilation.References)
            {
                if (context.Compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                {
                    if (assembly.Name.StartsWith(prefix))
                    {
                        yield return assembly;
                    }
                }
            }
        }
    }
}
