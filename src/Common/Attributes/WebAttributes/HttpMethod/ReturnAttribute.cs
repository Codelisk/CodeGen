using Shared.Models;

namespace Attributes.WebAttributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ReturnAttribute : Attribute
    {
        public ReturnKind ReturnKind { get; set; }
        public ReturnAttribute(ReturnKind returnKind)
        {
            ReturnKind = returnKind;
        }
    }
}
