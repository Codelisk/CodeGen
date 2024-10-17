using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class FieldDeclarationSyntaxExtensions
    {
        public static string GetFieldName(this FieldDeclarationSyntax fieldDeclaration)
        {
            // Extract all the variable names declared in this field
            return fieldDeclaration.Declaration.Variables.Select(v => v.Identifier.Text).First();
        }
    }
}
