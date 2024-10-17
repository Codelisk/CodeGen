using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class MethodDeclarationSyntaxExtensions
    {
        public static IEnumerable<AttributeSyntax> HttpAttribute(
            this MethodDeclarationSyntax method
        )
        {
            return method
                .GetAttributes()
                .Where(x => x.HasAttribute<GetFullAttribute>() || x.HasAttribute<GetAttribute>());
        }

        public static IEnumerable<AttributeSyntax> GetAttributes(
            this MethodDeclarationSyntax method
        )
        {
            return method.AttributeLists.SelectMany(attributeList => attributeList.Attributes);
        }

        public static bool HasAttribute(
            this MethodDeclarationSyntax methodDeclarationSyntax,
            Type type
        )
        {
            // Überprüfen, ob die Methode Attribute hat
            return methodDeclarationSyntax
                .AttributeLists.SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute => attribute.Name.ToString() == type.RealAttributeName());
        }

        public static bool HasAttribute<TAttribute>(
            this MethodDeclarationSyntax methodDeclarationSyntax
        )
        {
            // Überprüfen, ob die Methode Attribute hat
            return methodDeclarationSyntax
                .AttributeLists.SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute =>
                    attribute.Name.ToString() == typeof(TAttribute).RealAttributeName()
                );
        }

        public static string GetName(this MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.Identifier.Text;
        }

        /// <summary>
        /// Überprüft, ob die Methode ein Attribut hat, das von einem bestimmten Basistyp abgeleitet ist.
        /// </summary>
        /// <param name="methodDeclaration">Die Methodendeklaration, die überprüft werden soll.</param>
        /// <param name="baseTypeName">Der Name des Basistyps, nach dem gesucht werden soll.</param>
        /// <returns>Wahr, wenn die Methode ein Attribut hat, das von dem angegebenen Basistyp abgeleitet ist; andernfalls falsch.</returns>
        public static bool HasAttributeDerivedFrom(
            this MethodDeclarationSyntax methodDeclaration,
            string baseTypeName
        )
        {
            // Überprüfen Sie, ob die Methode Attributlisten hat
            return methodDeclaration
                .AttributeLists.SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute =>
                {
                    // Holen Sie sich den Namen des Attributs
                    var attributeName = attribute
                        .DescendantNodes()
                        .OfType<IdentifierNameSyntax>()
                        .FirstOrDefault()
                        ?.Identifier.Text;

                    // Vergleichen Sie den Namen des Attributs mit dem Basistypnamen
                    return attributeName != null && IsDerivedFrom(attributeName, baseTypeName);
                });
        }

        /// <summary>
        /// Überprüft, ob der Attributname vom angegebenen Basistyp abgeleitet ist.
        /// </summary>
        /// <param name="attributeName">Der Name des Attributs.</param>
        /// <param name="baseTypeName">Der Name des Basistyps.</param>
        /// <returns>Wahr, wenn der Attributname vom angegebenen Basistyp abgeleitet ist; andernfalls falsch.</returns>
        private static bool IsDerivedFrom(string attributeName, string baseTypeName)
        {
            // Überprüfen, ob der Attributname dem Basistypnamen entspricht
            // Diese Logik kann angepasst werden, um auch Abgleich mit abgeleiteten Typen zu ermöglichen
            return attributeName.Equals(baseTypeName, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
