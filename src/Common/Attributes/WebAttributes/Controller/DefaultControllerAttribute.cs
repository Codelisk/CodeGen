using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Controller
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultControllerAttribute : Attribute
    {
    }
}
