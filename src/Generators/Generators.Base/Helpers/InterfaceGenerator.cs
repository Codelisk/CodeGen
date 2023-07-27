using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Helpers
{
    public static class InterfaceGenerator
    {
        public static CodeBuilder GenerateInterface(this INamedTypeSymbol c, string nameSpace)
        {
            var codeBuilder = CodeBuilder.Create(nameSpace);

            var publicMethods = c.GetMethods().Where(x => x.DeclaredAccessibility == Accessibility.Public && !x.Name.Equals(".ctor"));

            var result = codeBuilder.AddClass("I" + c.Name).OfType(TypeKind.Interface).WithAccessModifier(Accessibility.Public);

            foreach (var publicMethod in publicMethods)
            {
                result.AddMethod(publicMethod.Name, Accessibility.NotApplicable)
                    .AddParameters(publicMethod.Parameters)
                    .WithReturnType(publicMethod.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                    .Abstract(true);
            }

            return codeBuilder;
        }
    }
}
