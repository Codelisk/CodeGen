using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;

namespace Codelisk.GeneratorAttributes.Helper
{
    public static class AttributeHelper
    {
        public static Dictionary<Type, string> AllAttributesMethodeHeaderDictionary(string delete="Delete", string get="Get", string post="Post")
        {
            return new Dictionary<Type, string>
            {
                {typeof(GetAttribute), get },
                {typeof(GetLastAttribute), get },
                {typeof(GetFullAttribute), get },
                {typeof(GetAllAttribute), get },
                {typeof(GetAllFullAttribute), get },
                {typeof(SaveAttribute), post },
                {typeof(AddAttribute), post },
                {typeof(AddRangeAttribute), post },
                {typeof(DeleteAttribute), delete }
            };
        }
    }
}
