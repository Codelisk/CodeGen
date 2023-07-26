using Attributes;
using Attributes.ApiAttributes;
using Attributes.WebAttributes.Database;
using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Crawler.Crawlers
{
    public static class AttributeCrawler
    {
        public static INamedTypeSymbol BaseApi(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseApiAttribute)).First();
        }
        public static INamedTypeSymbol BaseContext(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseContextAttribute)).First();
        }
        public static IEnumerable<INamedTypeSymbol> Dtos(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(DtoAttribute));
        }
        public static DefaultRepositoryModel DefaultRepository(this GeneratorExecutionContext context)
        {
            var repo= context.GetClassesWithAttribute(nameof(DefaultRepositoryAttribute)).First();

            return new DefaultRepositoryModel(repo,
                repo.GetMethodsWithAttribute(nameof(GetAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(GetAllAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(SaveAttribute)).First(),
            repo.GetMethodsWithAttribute(nameof(DeleteAttribute)).First());
        }
        public static INamedTypeSymbol BaseRepository(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseRepositoryAttribute)).First();
        }
        public static INamedTypeSymbol BaseController(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseControllerAttribute)).First();
        }
        public static IPropertySymbol GetIdProperty(this INamedTypeSymbol dto)
        {
            return dto.BaseType.GetPropertyWithAttribute(nameof(IdAttribute));
        }
    }
}
