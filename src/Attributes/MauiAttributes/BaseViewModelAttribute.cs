using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.MauiAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BaseViewModelAttribute : Attribute
    {
    }
}
