using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class UrlAttribute : Attribute
    {
        public string Url { get;}
        public UrlAttribute(string url)
        {
            this.Url = url;
        }
    }
}
