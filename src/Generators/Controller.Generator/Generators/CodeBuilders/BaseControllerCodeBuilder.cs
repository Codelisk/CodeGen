using Generator.Foundation.Generators.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Generator.Generators.CodeBuilders
{
    public abstract class BaseControllerCodeBuilder : BaseCodeBuilder
    {
        public BaseControllerCodeBuilder() : base(Constants.NameSpace)
        {
        }
    }
}
