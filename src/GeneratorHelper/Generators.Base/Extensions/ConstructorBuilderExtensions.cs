﻿using System.Reflection.Metadata;
using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Base.Extensions
{
    public static class ConstructorBuilderExtensions
    {
        public static ConstructorBuilder BaseConstructorTypeParameterParameterBaseCall(
            this ConstructorBuilder c,
            INamedTypeSymbol baseClass
        )
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

        public static ConstructorBuilder BaseConstructorParameterBaseCall(
            this ConstructorBuilder c,
            INamedTypeSymbol baseClass,
            (INamedTypeSymbol, string)? replaceTypeName = null
        )
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

        public static ConstructorBuilder AddParameterWithBaseCall(
            this ConstructorBuilder c,
            params (string type, string parameter)[] typesAndParameters
        )
        {
            Dictionary<string, string> typeParameters = new Dictionary<string, string>();
            foreach (var item in typesAndParameters)
            {
                c.AddParameter(item.type, item.parameter);

                typeParameters.Add(item.type, item.parameter);
            }
            // Call the base constructor with the collected type parameters
            c.WithBaseCall(typeParameters);

            return c;
        }

        public static ConstructorBuilder BaseConstructorParameterBaseCall(
            this ConstructorBuilder c,
            ClassDeclarationSyntax baseClass,
            params (string, string)[]? replaceTypeName
        )
        {
            // Get the first constructor from the base class
            var baseConstructor = baseClass
                .Members.OfType<ConstructorDeclarationSyntax>()
                .FirstOrDefault();

            // If no constructor found, we can't proceed
            if (baseConstructor == null)
            {
                return c;
            }

            Dictionary<string, string> typeParameters = new Dictionary<string, string>();

            // Iterate over the parameters of the base constructor
            foreach (var parameter in baseConstructor.ParameterList.Parameters)
            {
                // Extract type and parameter name
                var typeSyntax = parameter.Type;
                var parameterName = parameter.Identifier.Text;
                var typeName = typeSyntax.ToString();

                // Handle replacements if needed
                if (replaceTypeName is not null && replaceTypeName.Any())
                {
                    foreach (var item in replaceTypeName)
                    {
                        typeName = typeName.Replace(item.Item1, item.Item2);
                    }
                }

                // Add the type and parameter to the dictionary
                typeParameters.Add(typeName, parameterName);
            }

            // Call the base constructor with the collected type parameters
            c.WithBaseCall(typeParameters);

            return c;
        }
    }
}
