using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.GeneralAttributes.Registration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BaseRegisterAttribute : Attribute
    {
        public Type Interface { get; set; }
        public BaseRegisterAttribute()
        {
            
        }
        public BaseRegisterAttribute(Type interfaceType)
        {
            this.Interface = interfaceType;
        }
    }
}
