using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Generator.Models
{
    public class XamlExtract
    {
        public List<BindingElement> Bindings { get; set; } = new List<BindingElement>();
        public string ClassName { get; set; }
        public string ClassNamespace { get; set; }
    }
}
