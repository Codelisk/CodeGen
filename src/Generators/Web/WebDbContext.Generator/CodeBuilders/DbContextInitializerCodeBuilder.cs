using Generators.Base.CodeBuilders;

namespace WebDbContext.Generator.CodeBuilders
{
    public class DbContextInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public DbContextInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }
    }
}
