using System.Collections.Immutable;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Models
{
    public class ClassWithMethods
    {
        public ClassWithMethods(
            ClassDeclarationSyntax c,
            ImmutableArray<ClassDeclarationSyntax> baseTypes
        )
        {
            Class = c;

            Methods = c.GetMethodsWithBaseClasses(baseTypes).ToList();
        }

        public ClassDeclarationSyntax Class { get; set; }
        private List<MethodDeclarationSyntax> Methods { get; set; }

        public MethodDeclarationSyntax MethodFromAttribute<TAttribute>()
            where TAttribute : BaseHttpAttribute
        {
            foreach (var method in Methods)
            {
                if (method.HasAttribute(typeof(TAttribute).Name))
                {
                    return method;
                }
            }

            return null;
        }

        public MethodDeclarationSyntax MethodFromAttribute(Type attrType)
        {
            foreach (var method in Methods)
            {
                if (method.HasAttribute(attrType.Name))
                {
                    return method;
                }
            }

            return null;
        }
    }
}
