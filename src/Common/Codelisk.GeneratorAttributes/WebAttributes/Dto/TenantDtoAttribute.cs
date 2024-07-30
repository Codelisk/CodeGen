namespace Codelisk.GeneratorAttributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TenantDtoAttribute : Attribute
    {
        public string TenantName { get; set; }

        public TenantDtoAttribute(string name = "User")
        {
            TenantName = name;
        }
    }
}
