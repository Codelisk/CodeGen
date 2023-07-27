using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class GeneratorExecutionContextExtensions
    {
        public static INamedTypeSymbol GetClass<T>(this GeneratorExecutionContext context, string assemblyName)
        {
            return context.GetAllClasses(assemblyName).Single(x=>x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Equals(typeof(T).Name));
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass<T>(this GeneratorExecutionContext context)
        {
            var name = typeof(T).Name;
            return context.GetAllClasses("").Where(x =>x.BaseType?.Name is not null &&  x.BaseType.Name.Equals(name));
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass(this GeneratorExecutionContext context, INamedTypeSymbol baseClass)
        {
            return context.GetAllClasses("").Where(x=>x.BaseType == baseClass);
        }
        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttribute(this GeneratorExecutionContext context, string fullAttributeName)
        {
            return context.GetAllClasses("").Where(x => x.HasAttribute(fullAttributeName));
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
