using Attributes.GeneralAttributes.Registration;
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
        public static CodeBuilder GenerateInterface<TRegisterAttribute>(this CodeBuilder codeBuilder, GeneratorExecutionContext context) where TRegisterAttribute : BaseRegisterAttribute
        {
            var lastClass = codeBuilder.Classes.Last();
            lastClass.AddInterface("I" + lastClass.Name).AddAttribute(typeof(TRegisterAttribute).Name);
            codeBuilder.GetClasses(context).Last().GenerateInterface(codeBuilder, context);
            return codeBuilder;
        }
        public static CodeBuilder GenerateInterface(this INamedTypeSymbol c, CodeBuilder codeBuilder, GeneratorExecutionContext context)
        {
            var publicMethods = c.GetMethods().Where(x => x.DeclaredAccessibility == Accessibility.Public && !x.Name.Equals(".ctor"));

            var result = codeBuilder.AddClass("I" + c.Name).OfType(TypeKind.Interface).WithAccessModifier(Accessibility.Public);

            foreach (var publicMethod in publicMethods)
            {
                result.AddMethod(publicMethod.Name, Accessibility.NotApplicable)
                    .AddParameters(publicMethod.Parameters)
                    .WithReturnType(publicMethod.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                    .Abstract(true);
            }

            if (c.BaseType is not null)
            {
                var baseType = context.GetClassByName(c.BaseType.Name, "");
                foreach (var item1 in baseType.AllInterfaces)
                {
                    var finalInterface = item1;
                    if(item1.TypeArguments.Length > 0)
                    {
                        result.AddInterface(item1.OriginalDefinition.Construct(c.BaseType.TypeArguments.ToArray()).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                    }
                   // result.AddInterface(item1.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                }
            }

            return codeBuilder;
        }
    }
}
