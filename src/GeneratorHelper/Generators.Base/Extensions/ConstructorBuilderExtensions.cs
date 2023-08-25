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
                var typeName = parameter.Type.Name;
                string name = parameter.Type.Name.GetParameterName();
                if (replaceTypeName is not null)
                {
                    if (parameter.Type.Name.Equals(replaceTypeName.Value.Item1.Name))
                    {
                        typeName = typeName.Replace(parameter.Type.Name, replaceTypeName.Value.Item2);
                        name = replaceTypeName.Value.Item2.GetParameterName();
                    }
                    else if (parameter.Type.Name.Equals("I" + replaceTypeName.Value.Item1.Name))
                    {
                        typeName = typeName.Replace(parameter.Type.Name, "I" + replaceTypeName.Value.Item2);
                        name = replaceTypeName.Value.Item2.GetParameterName();
                    }
                }

                typeParameters.Add(typeName, name);
            }

            c.WithBaseCall(typeParameters);

            return c;
        }
    }
}
