namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class UrlAttribute : Attribute
    {
        public string Url { get; }
        public UrlAttribute(string url)
        {
            Url = url;
        }
    }
}
