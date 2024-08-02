using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class PropertyDeclarationSyntaxExtensions
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration syntax.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName(this PropertyDeclarationSyntax propertyDeclaration)
        {
            // Get the identifier token which contains the name of the property
            return propertyDeclaration.Identifier.Text;
        }

        /// <summary>
        /// Gets the type of the property as a string.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration syntax.</param>
        /// <returns>The type of the property as a string.</returns>
        public static string GetPropertyType(this PropertyDeclarationSyntax propertyDeclaration)
        {
            // Get the TypeSyntax representing the type of the property
            var typeSyntax = propertyDeclaration.Type;

            // Convert the TypeSyntax to a string
            return typeSyntax.ToString();
        }

        /// <summary>
        /// Gets the value of the first constructor argument of the specified attribute on the property.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration syntax.</param>
        /// <param name="attributeName">The name of the attribute to find.</param>
        /// <returns>The value of the first constructor argument of the attribute, or null if not found.</returns>
        public static string GetPropertyAttributeValue(
            this PropertyDeclarationSyntax propertyDeclaration,
            string attributeName
        )
        {
            // Find the attribute with the specified name
            var attribute = propertyDeclaration
                .AttributeLists.SelectMany(al => al.Attributes)
                .FirstOrDefault(attr => attr.Name.ToString() == attributeName.RealAttributeName());

            if (attribute == null)
            {
                return null;
            }

            // Get the value of the first constructor argument
            var argument = attribute.ArgumentList?.Arguments.FirstOrDefault();
            if (
                argument?.Expression is InvocationExpressionSyntax invocation
                && invocation.Expression is IdentifierNameSyntax identifierName
                && identifierName.Identifier.Text == "nameof"
            )
            {
                // Handle nameof(xyz) and return xyz
                var nameofArgument = invocation.ArgumentList.Arguments.FirstOrDefault();
                return nameofArgument?.ToString();
            }

            // Otherwise, return the expression as a string
            return argument?.Expression.ToString();
        }

        /// <summary>
        /// Checks if the property has the specified attribute.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration syntax.</param>
        /// <param name="attributeName">The name of the attribute to check for.</param>
        /// <returns>True if the property has the specified attribute; otherwise, false.</returns>
        public static bool HasAttribute(
            this PropertyDeclarationSyntax propertyDeclaration,
            string attributeName
        )
        {
            return propertyDeclaration
                .AttributeLists.SelectMany(al => al.Attributes)
                .Any(attr => attr.Name.ToString() == attributeName.RealAttributeName());
        }
    }
}
