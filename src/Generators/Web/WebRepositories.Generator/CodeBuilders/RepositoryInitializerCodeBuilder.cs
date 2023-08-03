using Generators.Base.CodeBuilders;

namespace WebRepositories.Generator.CodeBuilders
{
    public class RepositoryInitializerCodeBuilder : ClassServicesModuleInitializerBuilder
    {
        public RepositoryInitializerCodeBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }

        public override string ModuleName { get; set; } = "Repositories";
    }
}
