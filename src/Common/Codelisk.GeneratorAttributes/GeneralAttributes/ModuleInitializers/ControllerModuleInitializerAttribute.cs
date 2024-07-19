using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;

public class ControllerModuleInitializerAttribute : BaseModuleInitializerAttribute
{
    public ControllerModuleInitializerAttribute(string methodeName)
        : base(methodeName) { }
}
