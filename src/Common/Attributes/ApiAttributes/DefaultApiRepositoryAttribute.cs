namespace Attributes.ApiAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultApiRepositoryAttribute : Attribute
    {
    }
}
