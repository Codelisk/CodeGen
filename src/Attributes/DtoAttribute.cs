namespace Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DtoAttribute : Attribute
    {
        public string Name { get; set; } = "Dto";
    }
}