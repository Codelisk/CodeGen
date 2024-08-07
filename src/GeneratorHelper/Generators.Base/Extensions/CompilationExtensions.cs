﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions
{
    public static class CompilationExtensions
    {
        public static INamedTypeSymbol GetClassByName(
            this Compilation compilation,
            string name,
            string assemblyName
        )
        {
            return compilation.GetAllClasses(assemblyName).Single(x => x.Name.Equals(name));
        }

        public static INamedTypeSymbol GetClass(
            this Compilation compilation,
            Type t,
            string assemblyName
        )
        {
            return compilation.GetAllClasses(assemblyName).Single(x => x.Name == t.Name);
        }

        public static INamedTypeSymbol GetClass<T>(
            this Compilation compilation,
            string assemblyName
        )
        {
            return compilation
                .GetAllClasses(assemblyName)
                .Single(x =>
                    x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                        .Equals(typeof(T).Name)
                );
        }

        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass<T>(
            this Compilation compilation,
            string assemblyName = ""
        )
        {
            var name = typeof(T).Name;
            return compilation
                .GetAllClasses(assemblyName)
                .Where(x => x.BaseType?.Name is not null && x.BaseType.Name.Equals(name));
        }

        public static IEnumerable<INamedTypeSymbol> GetClassesWithBaseClass(
            this Compilation compilation,
            INamedTypeSymbol baseClass,
            string assemblyName = ""
        )
        {
            return compilation
                .GetAllClasses(assemblyName)
                .Where(x => SymbolEqualityComparer.Default.Equals(x.BaseType, baseClass));
        }

        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttributeOld(
            this Compilation compilation,
            string fullAttributeName,
            string assemblyName = ""
        )
        {
            var result = new List<INamedTypeSymbol>();
            INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName(fullAttributeName);
            if (attributeSymbol == null)
            {
                // Handle missing attribute symbol
                return null;
            }

            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                SemanticModel model = compilation.GetSemanticModel(syntaxTree);

                foreach (
                    var classNode in syntaxTree
                        .GetRoot()
                        .DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                )
                {
                    INamedTypeSymbol classSymbol =
                        model.GetDeclaredSymbol(classNode) as INamedTypeSymbol;
                    if (classSymbol != null)
                    {
                        foreach (var attribute in classSymbol.GetAttributes())
                        {
                            if (
                                attribute.AttributeClass.Equals(
                                    attributeSymbol,
                                    SymbolEqualityComparer.Default
                                )
                            )
                            {
                                // This class has the attribute, process as needed
                                result.Add(classSymbol);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttribute(
            this Compilation compilation,
            string fullAttributeName,
            string assemblyName = ""
        )
        {
            return compilation
                .GetAllClasses(assemblyName)
                .Where(x => x.HasAttributeWithoutBaseClass(fullAttributeName));
        }

        public static IEnumerable<INamedTypeSymbol> GetClassesWithAttributes(
            this Compilation compilation,
            string[] fullAttributeName,
            string assemblyName = ""
        )
        {
            return compilation
                .GetAllClasses(assemblyName)
                .Where(x => x.HasAttributeWithoutBaseClass(fullAttributeName));
        }

        private static Dictionary<(string, string), List<INamedTypeSymbol>> Classes = new();

        public static IEnumerable<INamedTypeSymbol> GetAllClasses(
            this Compilation compilation,
            string assemblyName
        )
        {
            //var key = (compilation.AssemblyName, assemblyName);
            //if (Classes.ContainsKey(key))
            //{
            //    return Classes[key];
            //}

            var result = new List<INamedTypeSymbol>();

            foreach (var assembly in compilation.GetAssembilesWithPrefix(assemblyName))
            {
                result.AddRange(assembly.GetAllTypeSymbols());
            }

            //Classes.Add(key, result);
            return result;
        }

        public static IEnumerable<IAssemblySymbol> GetAssembilesWithPrefix(
            this Compilation compilation,
            string prefix
        )
        {
            yield return compilation.Assembly;

            foreach (var reference in compilation.References)
            {
                if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
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
