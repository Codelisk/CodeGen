namespace Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DtoAttribute : Attribute
    {
        public DtoAttribute(string context = "BaseContext")
        {
            this.Context=context;
        }
        public string Context { get; set; }
    }
}