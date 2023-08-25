using Foundation.Crawler.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.RefitApis.Generator
{
    public static class Extensions
    {
        public static string ApiName(this INamedTypeSymbol dto)
        {
            return "I" + dto.ReplaceDtoSuffix() + "Api";
        }
    }
}
