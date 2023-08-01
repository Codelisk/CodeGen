using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.GeneratorAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BaseGeneratedAttribute : Attribute
    {
    }
}
