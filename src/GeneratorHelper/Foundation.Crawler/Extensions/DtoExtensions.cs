using Generators.Base;
using Microsoft.CodeAnalysis;

namespace Foundation.Crawler.Extensions
{
    public static class DtoExtensions
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
