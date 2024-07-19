using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.GeneralAttributes.ModuleInitializers;

public class RepositoryModuleInitializerAttribute : BaseModuleInitializerAttribute
{
    public RepositoryModuleInitializerAttribute(string methodeName)
        : base(methodeName) { }
}
