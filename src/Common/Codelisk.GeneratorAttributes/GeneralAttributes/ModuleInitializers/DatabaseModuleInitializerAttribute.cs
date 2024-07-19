using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;

public class DatabaseModuleInitializerAttribute : BaseModuleInitializerAttribute
{
    public DatabaseModuleInitializerAttribute(string methodeName)
        : base(methodeName) { }
}
