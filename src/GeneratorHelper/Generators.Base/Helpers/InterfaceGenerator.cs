using Codelisk.GeneratorAttributes.GeneralAttributes.Registration;
using CodeGenHelpers;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Helpers
{
    public static class InterfaceGenerator
    {
        public static CodeBuilder GenerateSeperateInterfaceCodeBuilder<TRegisterAttribute>(this CodeBuilder c, GeneratorExecutionContext context) where TRegisterAttribute : BaseRegisterAttribute
        {
            //var lastClass = c.Classes.Last();
            //lastClass.AddAttribute(typeof(TRegisterAttribute).Name);

            return c.GetClasses(context).Last().GenerateInterface(CodeBuilder.Create(context.Compilation.AssemblyName), context);
        }

        public static CodeBuilder GenerateInterface<TRegisterAttribute>(this CodeBuilder codeBuilder, GeneratorExecutionContext context) where TRegisterAttribute : Attribute
        {
            var lastClass = codeBuilder.Classes.Last();
            TestLog.Add("Adding " + lastClass.Name);
            lastClass.AddInterface("I" + lastClass.Name).AddAttribute(typeof(TRegisterAttribute).FullName);
            codeBuilder.GetClasses(context).Last().GenerateInterface(codeBuilder, context);
            return codeBuilder;
        }
        public static CodeBuilder GenerateInterface(this INamedTypeSymbol c, CodeBuilder codeBuilder, GeneratorExecutionContext context)
        {
            TestLog.Add("Start Generate");
            var publicMethods = c.GetMethods().Where(x => x.DeclaredAccessibility == Accessibility.Public && !x.Name.Equals(".ctor"));

            TestLog.Add("I" + c.Name);
            var result = codeBuilder.AddClass("I" + c.Name).OfType(TypeKind.Interface).WithAccessModifier(Accessibility.Public);

            List<string> nameSpacesFromUsedTypes = new List<string>();
            foreach (var publicMethod in publicMethods)
            {
                TestLog.Add("Method:" + publicMethod.Name);
                result.AddMethod(publicMethod.Name, Accessibility.NotApplicable)
                    .AddParameters(publicMethod.Parameters)
                    .WithReturnType(publicMethod.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                    .Abstract(true);

                TestLog.Add("publicMethod.ReturnType:" + publicMethod.ReturnType);
                nameSpacesFromUsedTypes.Add(publicMethod.ReturnType.GetNamespace());
                nameSpacesFromUsedTypes.AddRange(publicMethod.Parameters.Select(x => x.Type.GetNamespace()));
            }


            TestLog.Add("c.BaseType" + c.BaseType);
            if (c.BaseType is not null)
            {
                var baseType = context.GetClassByName(c.BaseType.Name, "");
                var interFace = baseType.Interfaces.FirstOrDefault();
                if (interFace is not null)
                {
                    TestLog.Add("nameSpacesFromUsedTypes.Add(interFace.GetNamespace());");
                    nameSpacesFromUsedTypes.Add(interFace.GetNamespace());
                    if (interFace.TypeArguments.Length > 0 && interFace.TypeArguments.Length == c.BaseType.TypeArguments.Length)
                    {
                        nameSpacesFromUsedTypes.AddRange(interFace.TypeArguments.Select(x => x.GetNamespace()));
                        result.AddInterface(interFace.OriginalDefinition.Construct(c.BaseType.TypeArguments.ToArray()).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                    }
                    else if (interFace.TypeArguments.Length > 0)
                    {
                        List<ITypeSymbol> types = new List<ITypeSymbol>();
                        for (int i = 0; i < interFace.TypeArguments.Length; i++)
                        {
                            types.Add(c.BaseType.TypeArguments[i]);
                        }

                        result.AddInterface(interFace.OriginalDefinition.Construct(types.ToArray()).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                    }
                    else
                    {
                        result.AddInterface(interFace);
                    }
                    TestLog.Add("NICE");
                }
            }

            try
            {
                nameSpacesFromUsedTypes.Distinct().ToList().ForEach(x => result.AddNamespaceImport(x));
            }
            catch(Exception ex)
            {
                TestLog.Add(ex.Message + " INNER" + ex.InnerException?.Message);
            }

            return codeBuilder;
        }
    }
}
