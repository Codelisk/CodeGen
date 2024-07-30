namespace Codelisk.GeneratorAttributes.WebAttributes.Manager
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultManagerAttribute : Attribute
    {
        public string? TenantName { get; set; }

        public DefaultManagerAttribute(string? tenantName = null)
        {
            TenantName = tenantName;
        }
    }
}
