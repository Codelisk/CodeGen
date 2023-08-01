using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class BaseHttpAttribute : Attribute
    {
    }
}
