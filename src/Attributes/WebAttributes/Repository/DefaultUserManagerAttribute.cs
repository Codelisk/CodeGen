using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DefaultUserManagerAttribute : Attribute
    {
    }
}
