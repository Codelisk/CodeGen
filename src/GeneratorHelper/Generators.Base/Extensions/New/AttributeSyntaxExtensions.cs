using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class AttributeSyntaxExtensions
    {
        /// <summary>
        /// Checks if the current attribute has another specified attribute of type TAttribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to check for.</typeparam>
        /// <param name="attributeSyntax">The attribute syntax to check.</param>
        /// <returns>True if the attribute has the specified attribute; otherwise, false.</returns>
        public static bool HasAttribute<TAttribute>(this AttributeSyntax attributeSyntax)
            where TAttribute : Attribute
        {
            // Get the full name of the attribute type
            var attributeName = typeof(TAttribute).Name;

            // Check if the attributeSyntax has any attribute lists
            var attributeLists = attributeSyntax.Parent as AttributeListSyntax;

            if (attributeLists == null)
                return false; // No attribute lists to check against

            // Check if any attribute in the list matches the specified attribute name
            return attributeLists.Attributes.Any(attr =>
                attr.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .FirstOrDefault()
                    ?.Identifier.Text.Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                == true
            );
        }

        public static string GetFirstConstructorArgument(this AttributeSyntax? attribute)
        {
            if (attribute == null)
            {
                return null; // or throw an exception if that's preferred
            }

            // Get the constructor argument(s) from the attribute
            var arguments = attribute.ArgumentList?.Arguments;

            // Return the first argument as a string
            return arguments?.Count > 0 ? arguments.Value[0].ToString() : null;
        }
    }
}
