using CodeGenHelpers;
using Microsoft.CodeAnalysis;

namespace Generators.Base.Extensions
{
    public static class ClassBuilderExtensions
    {
        //public static void AddMissingNamespaceImports(this ClassBuilder classBuilder, GeneratorExecutionContext context)
        //{
        //    var classes = classBuilder.GetClasses(context);

        //    foreach ( var clazz in classes)
        //    {
        //        clazz.GetTypeMembers().ToList().ForEach(typeMember =>
        //        {
        //            var namespaceName = typeMember.GetNamespace();
        //            classBuilder.AddNamespaceImport(namespaceName);
        //        });
        //    }
        //}
        public static void AddMissingNamespaceImports(this CodeBuilder codeBuilder, GeneratorExecutionContext context)
        {
            List<string> imports = new List<string>();
            var classes = codeBuilder.GetClasses(context);

            foreach (var clazz in classes)
            {
                clazz.GetTypeMembers().ToList().ForEach(typeMember =>
                {
                    var namespaceName = typeMember.GetNamespace();
                    imports.Add(namespaceName);
                });
                if (clazz.BaseType is not null)
                {
                    imports.Add(clazz.BaseType.GetNamespace());
                    if (clazz.BaseType.TypeArguments.Length > 0)
                    {
                        imports.AddRange(clazz.BaseType.TypeArguments.Select(x => x.GetNamespace()));
                    }
                }

                foreach (var import in imports)
                {
                    if (import is not null)
                    {
                        codeBuilder.AddNamespaceImport(import);
                    }
                }
            }
        }
    }
}
