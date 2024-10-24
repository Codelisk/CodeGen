using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base.Extensions.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions.New
{
    public static class InterfaceDeclarationSyntaxExtensions
    {
        public static bool HasAttribute<TAttribute>(
            this InterfaceDeclarationSyntax RecordDeclarationSyntax
        )
        {
            // Check if the type declaration has the BaseContext attribute
            foreach (var attributeList in RecordDeclarationSyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    var sdf = attribute.Name.ToFullString();
                    if (attribute.Name.ToString() == typeof(TAttribute).RealAttributeName())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
