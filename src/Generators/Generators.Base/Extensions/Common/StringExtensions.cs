using System;
using System.Collections.Generic;
using System.Text;

namespace Generators.Base.Extensions.Common
{
    public static class StringExtensions
    {
        public static string AttributeWithConstructor(this string constructorValue, string attributeName)
        {
            return $"{attributeName}(\"{constructorValue}\")";
        }
    }
}
