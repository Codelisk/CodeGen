using Generators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Crawler
{
    public static class Extensions
    {
        public static string ReplaceDtoSuffix(this INamedTypeSymbol dto, bool plural = false)
        {
            var name = dto.Name.Replace("Dto", "");
            if (plural)
            {
                name = name.Pluralize();
            }
            return name;
        }
    }
}
