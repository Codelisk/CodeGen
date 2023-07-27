﻿using Foundation.Crawler;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Generator
{
    public static class Extensions
    {
        public static string ApiName(this INamedTypeSymbol dto)
        {
            return "I" + dto.ReplaceDtoSuffix() + "Api";
        }
        public static string RefitInterfaceNameFromController(this INamedTypeSymbol controller)
        {
            return "I" + controller.Name.Replace("Controller", "") + "Api";
        }
        public static string RepositoryNameFromApi(this INamedTypeSymbol api)
        {
            return api.Name.Substring(1, api.Name.Length - 1) + "Repository";
        }
        public static string RepositoryInterfaceNameFromApi(this INamedTypeSymbol api)
        {
            return "I" + RepositoryNameFromApi(api);
        }
    }
}
