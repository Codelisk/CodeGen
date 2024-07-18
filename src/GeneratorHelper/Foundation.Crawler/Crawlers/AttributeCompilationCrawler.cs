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

        public string? GetInitNamespace<TInitAttribute>()
            where TInitAttribute : BaseModuleInitializerAttribute
        {
            var result = context
                .GetClassesWithAttribute(typeof(TInitAttribute).Name)
                .FirstOrDefault()
                ?.GetNamespace();

            if (string.IsNullOrEmpty(result))
            {
                return context.AssemblyName;
            }
            return result;
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
            return context.GetClassesWithAttribute(nameof(DtoAttribute));
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

        public INamedTypeSymbol Manager(INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultManagerAttribute>(dto);
        }

        public INamedTypeSymbol Controller(INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultControllerAttribute>(dto);
        }

        public INamedTypeSymbol Repository(INamedTypeSymbol dto)
        {
            return UserOrDefault<DefaultRepositoryAttribute>(dto);
        }

        //For caching
        private IEnumerable<INamedTypeSymbol> classSymbols;

        private INamedTypeSymbol UserOrDefault<TAttribute>(
            INamedTypeSymbol dto,
            bool isUser = false
        )
            where TAttribute : Attribute
        {
            classSymbols = context.GetAllClasses("");
            var objectsWithAttribute = classSymbols.GetClassesWithAttribute(
                typeof(TAttribute).Name
            );
            if (dto.HasAttribute(nameof(UserDtoAttribute)))
            {
                return objectsWithAttribute.FirstOrDefault(x =>
                    x.HasAttribute(nameof(UserDtoAttribute))
                );
            }
            else
            {
                return objectsWithAttribute.FirstOrDefault(x =>
                    !x.HasAttribute(nameof(UserDtoAttribute))
                );
            }
        }

        public Compilation GetCompilation()
        {
            return context;
        }
    }
}
