using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class BaseModuleInitializerAttribute : Attribute
{
    public string MethodeName { get; set; }

    public BaseModuleInitializerAttribute(string methodeName = "AddServices")
    {
        this.MethodeName = methodeName;
    }
}
