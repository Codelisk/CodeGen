using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Generator.Models
{
    public class BindingElement
    {
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public string ElementName { get; set; }
        public string? ElementNamespace { get; set; }
        public string DataType { get; set; }
        public bool IsCustomType { get; set; }
    }
}
