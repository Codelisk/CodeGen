using CodeGenHelpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions
{
    public static class ConstructorBuilderExtensions
    {

        public static ConstructorBuilder BaseConstructorTypeParameterParameterBaseCall(this ConstructorBuilder c, INamedTypeSymbol baseClass)
        {
            var baseConstructor = baseClass.InstanceConstructors.First();
            Dictionary<string, string> typeParameters = new Dictionary<string, string>();
            foreach (var parameter in baseConstructor.Parameters)
            {
                var typeName = parameter.Type.ToDisplayString();
                string name = parameter.Type.Name.GetParameterName();
                typeParameters.Add(typeName, name);
            }

            c.WithBaseCall(typeParameters);

            return c;
        }
        public static ConstructorBuilder BaseConstructorParameterBaseCall(this ConstructorBuilder c, INamedTypeSymbol baseClass, string? replaceTypeName = null)
        {
            var baseConstructor = baseClass.InstanceConstructors.First();
            Dictionary<string, string> typeParameters = new Dictionary<string, string>();
            foreach (var parameter in baseConstructor.Parameters)
            {
                var typeName = parameter.Type.Name;
                string name = parameter.Type.Name.GetParameterName();
                if (replaceTypeName is not null)
                {
                    typeName = typeName.Replace(parameter.Type.Name, replaceTypeName);
                }
                if(replaceTypeName is not null)
                {
                    name = replaceTypeName.GetParameterName();
                }
                typeParameters.Add(typeName, name);
            }

            c.WithBaseCall(typeParameters);

            return c;
        }
    }
}
