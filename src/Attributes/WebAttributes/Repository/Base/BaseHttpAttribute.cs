using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository.Base
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class BaseHttpAttribute : Attribute
    {
        [Url]
        public abstract string UrlPrefix { get; }
    }
}
