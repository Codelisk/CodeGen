using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Generators.Base.Extensions;
using Generators.Base.Extensions.New;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foundation.Crawler.Extensions.New.AttributeFinder
{
    public static class CommonAttributeFinderExtensions
    {
        public static ClassDeclarationSyntax TenantOrDefault<TAttribute>(
            this RecordDeclarationSyntax dto,
            IEnumerable<ClassDeclarationSyntax> classSymbols,
            bool isUser = false
        )
            where TAttribute : Attribute
        {
            // Extract classes with the target attribute
            var objectsWithAttribute = classSymbols
                .Where(x => x.HasAttribute<TAttribute>())
                .ToList();

            // Check if the dto has TenantDtoAttribute
            if (dto.HasAttribute<TenantDtoAttribute>())
            {
                // Extract the tenant name from the TenantDtoAttribute
                string tenantName = dto.GetAttribute<TenantDtoAttribute>()
                    .GetFirstConstructorArgument();

                // Find the class with the matching tenant attribute name
                return objectsWithAttribute.FirstOrDefault(x =>
                {
                    var tenantAttr = x.GetAttribute<TAttribute>();
                    if (tenantAttr is null)
                    {
                        return false;
                    }
                    var tenantAttrName = tenantAttr.GetFirstConstructorArgument();
                    return tenantAttrName == tenantName;
                });
            }
            else
            {
                // Return the first class that has an empty tenant attribute argument
                return objectsWithAttribute.FirstOrDefault(x =>
                    string.IsNullOrEmpty(x.GetAttribute<TAttribute>().GetFirstConstructorArgument())
                );
            }
        }
    }
}
