using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class TypeSyntaxExtensions
    {
        public static string GetName(this TypeSyntax typeSyntax)
        {
            // Überprüfe, ob es ein IdentifierNameSyntax ist
            if (typeSyntax is IdentifierNameSyntax identifierName)
            {
                return identifierName.Identifier.Text;
            }

            // Überprüfe, ob es ein QualifiedNameSyntax ist (z.B. Namespace.Class)
            if (typeSyntax is QualifiedNameSyntax qualifiedName)
            {
                return qualifiedName.ToString(); // Oder qualifiedName.Right.Identifier.Text; je nach Anforderung
            }

            // Überprüfe, ob es ein GenericNameSyntax ist (z.B. List<string>)
            if (typeSyntax is GenericNameSyntax genericName)
            {
                return genericName.Identifier.Text; // Der Name der generischen Klasse
            }

            // Weitere Typen könnten hier hinzugefügt werden

            return null; // Rückgabe, wenn kein passender Typ gefunden wurde
        }
    }
}
