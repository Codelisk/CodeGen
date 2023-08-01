using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class IdQueryAttribute : Attribute
    {
    }
}
