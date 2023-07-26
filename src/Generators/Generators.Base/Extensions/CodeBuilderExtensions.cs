using CodeGenHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class CodeBuilderExtensions
    {
        public static List<INamedTypeSymbol> GetClasses(this CodeBuilder codeBuilder)
        {
            var test = codeBuilder.Build();
            var syntaxTree = CSharpSyntaxTree.ParseText(codeBuilder.Build());
            var compilation = CSharpCompilation.Create("MyCompilation", new[] { syntaxTree });

            var root = syntaxTree.GetRoot();
            var interfaceDeclarations = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>();

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
