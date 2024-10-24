using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class DtoBaseInterfaceAttribute : Attribute { }
}
