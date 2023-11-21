using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RemoveGetAll : Attribute
    {
    }
}
