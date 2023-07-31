using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    public class EntityAttribute : BaseEntityAttribute
    {
        public Type Type { get; set; }
        public EntityAttribute(Type type)
        {
            Type = type;
        }
    }
}
