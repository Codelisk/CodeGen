using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Dto
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class IdAttribute : Attribute
    {
    }
}
