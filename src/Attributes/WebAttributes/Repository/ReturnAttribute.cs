using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ReturnAttribute : Attribute
    {
        public ReturnKind ReturnKind { get; set; }
        public ReturnAttribute(ReturnKind returnKind)
        {
            this.ReturnKind = returnKind;
        }
    }
}
