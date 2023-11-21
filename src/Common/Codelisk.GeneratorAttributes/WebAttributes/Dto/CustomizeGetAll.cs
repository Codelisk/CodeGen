using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CustomizeGetAll : Attribute
    {
        public bool AllowAnonymous { get; set; }
    }
}
