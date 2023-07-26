using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DeleteAttribute : Attribute
    {
    }
}
