using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Manager
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultManagerAttribute : Attribute
    {
    }
}
