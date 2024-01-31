using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Microsoft.CodeAnalysis;

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
        public static ConstructorBuilder BaseConstructorParameterBaseCall(this ConstructorBuilder c, INamedTypeSymbol baseClass, (INamedTypeSymbol, string)? replaceTypeName = null)
        {
            var baseConstructor = baseClass.InstanceConstructors.First();
            Dictionary<string, string> typeParameters = new Dictionary<string, string>();
            foreach (var parameter in baseConstructor.Parameters)
            {
                var type = parameter.Type;
                string typeName = type.Name;
                string name = type.Name.GetParameterName();
                if (replaceTypeName is not null)
                {
                    if (type.Name.Equals(replaceTypeName.Value.Item1.Name))
                    {
                        typeName = typeName.Replace(type.Name, replaceTypeName.Value.Item2);
                        name = replaceTypeName.Value.Item2.GetParameterName();
                    }
                    else if (type.Name.Equals("I" + replaceTypeName.Value.Item1.Name))
                    {
                        typeName = typeName.Replace(type.Name, "I" + replaceTypeName.Value.Item2);
                        name = replaceTypeName.Value.Item2.GetParameterName();
                    }
                }

                //if typename has not been replaced use full
                if (typeName.Equals(type.Name))
                {
                    typeName = type.GetFullName();
                }

                typeParameters.Add(typeName, name);
            }

            c.WithBaseCall(typeParameters);

            return c;
        }
    }
}
