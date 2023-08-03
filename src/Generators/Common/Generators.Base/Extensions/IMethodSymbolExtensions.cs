using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

                            if (result is not null)
                            {
                                return result;
                            }
                        }
                    }

                    if (genericTypeArgument is not null)
                    {
                        return genericTypeArgument;
                    }
                }
            }

            return context.Compilation.GetSpecialType(SpecialType.System_String); // Return null if the DoWithLoggingAsync<int>() method call is not found or if the return type is not available
        }
        public static string GetPropertyOfAttribute<TAttribute, TPropertyAttribute>(this IMethodSymbol methodSymbol) where TAttribute : Attribute where TPropertyAttribute : Attribute
        {
            // Get the attributes applied to the method
            var attributes = methodSymbol.GetAttributes();

            // Find the custom attribute of type GetAttribute
            INamedTypeSymbol getAttributeType = methodSymbol.ContainingAssembly
                .GetTypeByMetadataName(nameof(TAttribute));

            var getAttributeData = attributes.FirstOrDefault(a => a.AttributeClass.Equals(getAttributeType));

            if (getAttributeData != null)
            {
                // Find the property with the Url attribute
                var urlProperty = getAttributeType.GetMembers().OfType<IPropertySymbol>()
                    .FirstOrDefault(property => property.GetAttributes().Any(attr => attr.AttributeClass.Name == nameof(TPropertyAttribute)));

                if (urlProperty != null)
                {
                    // Get the 'UrlPrefix' property value from the GetAttribute
                    string urlPrefix = urlProperty.GetAttributes()
                        .FirstOrDefault(attr => attr.AttributeClass.Name == nameof(TPropertyAttribute))
                        .ConstructorArguments.FirstOrDefault().Value.ToString();

                    return urlPrefix;
                }
            }

            return null; // If the UrlPrefix is not found or GetAttribute is not applied.
        }
        public static AttributeData GetAttributeWithBaseType(this IMethodSymbol methodSymbol, Type type)
        {
            return methodSymbol.GetAttributes().FirstOrDefault(x => x.AttributeClass.BaseType.Name == type.Name);
        }
        public static AttributeData GetAttributeByName(this IMethodSymbol methodSymbol, string attributeName)
        {
            return methodSymbol.GetAttributes().FirstOrDefault(x => x.GetAttributeName().Equals(attributeName));
        }
        public static bool HasAttribute(this IMethodSymbol methodSymbol, string attributeName)
        {
            return methodSymbol.GetAttributes().Any(x => x.GetAttributeName().Equals(attributeName));
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
