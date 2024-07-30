namespace Codelisk.GeneratorAttributes.WebAttributes.Repository
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultRepositoryAttribute : Attribute
    {
        public string? TenantName { get; set; }

        public DefaultRepositoryAttribute(string? tenantName = null)
        {
            this.TenantName = tenantName;
        }
    }
}
