namespace Codelisk.GeneratorAttributes.WebAttributes.Controller
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultControllerAttribute : Attribute
    {
        public string? TenantName { get; set; }

        public DefaultControllerAttribute(string? tenantName = null)
        {
            this.TenantName = tenantName;
        }
    }
}
