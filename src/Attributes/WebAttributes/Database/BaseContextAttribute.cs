using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Database
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BaseContextAttribute : Attribute
    {
    }
}
