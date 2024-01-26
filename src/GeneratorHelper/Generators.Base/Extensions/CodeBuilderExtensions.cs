using CodeGenHelpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions
{
    public static class CodeBuilderExtensions
    {
        public static List<INamedTypeSymbol> GetClasses(this ClassBuilder classBuilder, Compilation compilation)
        {
            return classBuilder.Build().GetClasses(compilation);
        }

        public static List<INamedTypeSymbol> GetClasses(this CodeBuilder codeBuilder, Compilation compilation)
        {
            return codeBuilder.Build().GetClasses(compilation);
        }

        private static List<INamedTypeSymbol> GetClasses(this string code, Compilation compilation)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var newCompilation = CSharpCompilation.Create(compilation.AssemblyName, new[] { syntaxTree });

            var root = syntaxTree.GetRoot();
            var interfaceDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            List<INamedTypeSymbol> result = new List<INamedTypeSymbol>();

            foreach (var i in interfaceDeclarations)
            {
                if (i != null)
                {
                    // Get the semantic model for the syntax tree
                    var semanticModel = compilation.GetSemanticModel(syntaxTree);

                    // Get the symbol representing the class
                    var classSymbol = semanticModel.GetDeclaredSymbol(i) as INamedTypeSymbol;
                    result.Add(classSymbol);
                }
            }
            return result;
        }

    }
}
