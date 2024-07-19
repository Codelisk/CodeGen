using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;

public class ManagerModuleInitializerAttribute : BaseModuleInitializerAttribute
{
    public ManagerModuleInitializerAttribute(string methodeName)
        : base(methodeName) { }
}
