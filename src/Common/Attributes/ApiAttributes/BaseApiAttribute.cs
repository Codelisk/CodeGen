using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.ApiAttributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class BaseApiAttribute : Attribute
    {
    }
}
