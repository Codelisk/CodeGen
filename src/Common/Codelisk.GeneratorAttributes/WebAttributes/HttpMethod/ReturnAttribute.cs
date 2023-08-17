using Shared.Models;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
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
