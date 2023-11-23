using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class GetIdAttribute : Attribute
    {
    }
}
