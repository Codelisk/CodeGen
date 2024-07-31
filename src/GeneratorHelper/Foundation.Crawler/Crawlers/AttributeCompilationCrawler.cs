using Codelisk.GeneratorAttributes;
using Codelisk.GeneratorAttributes.ApiAttributes;
using Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;
using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Database;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Foundation.Crawler.Extensions;
using Foundation.Crawler.Models;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;

namespace Foundation.Crawler.Crawlers
{
    public class AttributeCompilationCrawler
    {
        Compilation context;

        public AttributeCompilationCrawler(Compilation compilation)
        {
            context = compilation;
        }

        public (string?, string?) NameSpaceAndMethod<TInitAttribute>()
            where TInitAttribute : BaseModuleInitializerAttribute
        {
            var symbol = context
                .GetClassesWithAttribute(typeof(TInitAttribute).Name)
                .FirstOrDefault();
            return (
                GetInitNamespace<TInitAttribute>(symbol),
                GetInitMethodeName<TInitAttribute>(symbol)
            );
        }

        private string? GetInitNamespace<TInitAttribute>(INamedTypeSymbol symbol)
            where TInitAttribute : BaseModuleInitializerAttribute
        {
            var result = symbol.GetNamespace();

            if (string.IsNullOrEmpty(result))
            {
                return context.AssemblyName;
            }
            return result;
        }

        private string? GetInitMethodeName<TInitAttribute>(INamedTypeSymbol symbol)
            where TInitAttribute : BaseModuleInitializerAttribute
        {
            if (symbol is not null)
            {
                return symbol.GetAttribute<TInitAttribute>().GetFirstConstructorArgument();
            }
            return null;
        }

        public INamedTypeSymbol BaseApi()
        {
            return context.GetClassesWithAttribute(nameof(BaseApiAttribute)).First();
        }

        public INamedTypeSymbol BaseContext()
        {
            return context.GetClassesWithAttribute(nameof(BaseContextAttribute)).First();
        }

        public IEnumerable<INamedTypeSymbol> Dtos()
        {
            return context.GetClassesWithAttributes(
                new string[] { nameof(TenantDtoAttribute), nameof(DtoAttribute) }
            );
        }

        public IEnumerable<INamedTypeSymbol> Entities()
        {
            return context.GetClassesWithAttribute(nameof(EntityAttribute));
        }

        public INamedTypeSymbol DefaultApiRepository()
        {
            return context.GetClassesWithAttribute(nameof(DefaultApiRepositoryAttribute)).First();
        }

        public INamedTypeSymbol GetAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            // Find the property with the Url attribute
            return context.GetClass<TAttribute>("Codelisk.GeneratorAttributes");
        }

        public string AttributeUrl(Type t, INamedTypeSymbol dto)
        {
            // Find the property with the Url attribute
            var attributeSymobl = context.GetClass(t, "Codelisk.GeneratorAttributes");
            return attributeSymobl.AttributeUrl(dto);
        }

        public string AttributeUrl<TAttribute>(INamedTypeSymbol dto)
            where TAttribute : BaseHttpAttribute
        {
            // Find the property with the Url attribute
            var attributeSymobl = GetAttribute<TAttribute>();
            return attributeSymobl.AttributeUrl(dto);
        }

        public INamedTypeSymbol Manager(INamedTypeSymbol dto, string assemblyName)
        {
            return TenantOrDefault<DefaultManagerAttribute>(dto, assemblyName: assemblyName);
        }

        public INamedTypeSymbol Controller(INamedTypeSymbol dto, string assemblyName)
        {
            return TenantOrDefault<DefaultControllerAttribute>(dto, assemblyName: assemblyName);
        }

        public INamedTypeSymbol Repository(INamedTypeSymbol dto, string assemblyName)
        {
            return TenantOrDefault<DefaultRepositoryAttribute>(dto, assemblyName: assemblyName);
        }

        //For caching
        private IEnumerable<INamedTypeSymbol> classSymbols;

        private INamedTypeSymbol TenantOrDefault<TAttribute>(
            INamedTypeSymbol dto,
            bool isUser = false,
            string assemblyName = ""
        )
            where TAttribute : Attribute
        {
            classSymbols = context.GetAllClasses(assemblyName);
            var objectsWithAttribute = classSymbols.GetClassesWithAttribute(
                typeof(TAttribute).Name
            );
            if (dto.HasAttribute(nameof(TenantDtoAttribute)))
            {
                string tenantName = dto.GetAttribute<TenantDtoAttribute>()
                    .GetFirstConstructorArgument();
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
                return objectsWithAttribute.FirstOrDefault(x =>
                    string.IsNullOrEmpty(x.GetAttribute<TAttribute>().GetFirstConstructorArgument())
                );
            }
        }

        public Compilation GetCompilation()
        {
            return context;
        }
    }
}
