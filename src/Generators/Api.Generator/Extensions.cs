using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Generator
{
    public static class Extensions
    {
        public static string RefitInterfaceName(this INamedTypeSymbol controller)
        {
            return "I" + controller.Name.Replace("Controller", "") + "Api";
        }
        public static string RepositoryName(this INamedTypeSymbol api)
        {
            return api.Name.Substring(1, api.Name.Length - 1) + "Repository";
        }
        public static string RepositoryInterfaceName(this INamedTypeSymbol api)
        {
            return "I" + RepositoryName(api);
        }
    }
}
