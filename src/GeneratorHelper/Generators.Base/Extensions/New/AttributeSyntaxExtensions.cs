using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class AttributeSyntaxExtensions
    {
        public static bool HasArgument(this AttributeSyntax attribute, string argument)
        {
            var arguments = attribute.GetAttributeArguments();

            return arguments.ContainsKey(argument);
        }

        public static Dictionary<string, string> GetAttributeArguments(
            this AttributeSyntax attribute
        )
        {
            var arguments = new Dictionary<string, string>();

            // Retrieve constructor arguments (positional arguments)
            var constructorArguments = attribute.ArgumentList?.Arguments;
            if (constructorArguments != null)
            {
                int index = 0;
                foreach (var arg in constructorArguments)
                {
                    // Add argument position as the key
                    arguments.Add($"Arg{index++}", arg.ToString());
                }
            }

            // Retrieve named arguments (e.g., MyAttribute(Name = "value"))
            if (attribute.ArgumentList != null)
            {
                foreach (var arg in attribute.ArgumentList.Arguments)
                {
                    if (arg.NameEquals != null) // Check for named arguments
                    {
                        var name = arg.NameEquals.Name.ToString();
                        var value = arg.Expression.ToString();
                        arguments.Add(name, value);
                    }
                }
            }

            return arguments;
        }

        public static string AttributeUrl(this string attributeValue, RecordDeclarationSyntax dto)
        {
            //var attribute = context.GetClassesWithAttribute(nameof(UrlAttribute)).OfType<TAttribute>().First();
            return $"{attributeValue}";
        }

        public static string AttributeUrl(
            this ClassDeclarationSyntax attributeSymobl,
            RecordDeclarationSyntax dto
        )
        {
            var urlProperty = attributeSymobl.GetAttribute<UrlAttribute>();
            bool plural = attributeSymobl.HasAttribute<PluralAttribute>();
            return urlProperty.GetFirstConstructorArgument().AttributeUrl(dto);
        }

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
