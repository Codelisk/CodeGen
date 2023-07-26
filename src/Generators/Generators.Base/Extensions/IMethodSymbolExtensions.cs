using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class IMethodSymbolExtensions
    {
        // Get the specific return type from the method invocation
        public static ITypeSymbol GetReturnTypeOrString(this IMethodSymbol method, GeneratorExecutionContext context, bool forceGetDto)
        {
            var methodDeclaration = method.GetMethodDeclarationSyntax();
            var compilation = context.Compilation;
            var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);

            // Find the invocation expression of DoWithLoggingAsync method
            var invocationExpressions = methodDeclaration.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocationExpression in invocationExpressions)
            {
                var methodSymbol = semanticModel.GetSymbolInfo(invocationExpression).Symbol as IMethodSymbol;
                if ((methodSymbol?.Name == "DoWithLoggingAsync" || methodSymbol?.Name == "OK") && methodSymbol?.TypeArguments.Length == 1)
                {
                    // Extract the generic type argument from the method invocation
                    var genericTypeArgument = methodSymbol.TypeArguments[0];

                    // If the generic type argument is IEnumerable<> (or inherits from it), get the element type
                    if (forceGetDto && genericTypeArgument.IsEnumerableType(compilation))
                    {
                        if (genericTypeArgument is not null && genericTypeArgument is INamedTypeSymbol elementType)
                        {
                            var result = elementType.GetEnumerableElementType();

                            if(result is not null)
                            {
                                return result;
                            }
                        }
                    }

                    if(genericTypeArgument is not null)
                    {
                        return genericTypeArgument;
                    }
                }
            }

            return context.Compilation.GetSpecialType(SpecialType.System_String); // Return null if the DoWithLoggingAsync<int>() method call is not found or if the return type is not available
        }
        public static AttributeData GetAttributeByName(this IMethodSymbol methodSymbol, string attributeName)
        {
            return methodSymbol.GetAttributes().FirstOrDefault(x => x.GetAttributeName().Equals(attributeName));
        }
        public static MethodDeclarationSyntax GetMethodDeclarationSyntax(this IMethodSymbol methodSymbol)
        {
            var methodSyntaxReferences = methodSymbol.DeclaringSyntaxReferences;
            var methodSyntaxReference = methodSyntaxReferences.FirstOrDefault();
            if (methodSyntaxReference != null)
            {
                return methodSyntaxReference.GetSyntax() as MethodDeclarationSyntax;
            }

            return null; // Return null if the method declaration syntax is not available
        }
    }
}
