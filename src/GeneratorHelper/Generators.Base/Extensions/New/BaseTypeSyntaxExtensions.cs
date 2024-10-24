using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class BaseTypeSyntaxExtensions
    {
        public static string GetFullTypeName(this BaseTypeSyntax baseType)
        {
            var result = baseType.GetNamespace() + "." + baseType.Type.ToString();
            return result;
        }
    }
}
