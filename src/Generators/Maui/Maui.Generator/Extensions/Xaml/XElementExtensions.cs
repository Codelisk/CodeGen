using Maui.Generator.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Maui.Generator.Extensions.Xaml
{
    public static class XElementExtensions
    {
        public static string GetDataType(this XElement rootNode, string dataTypeValue)
        {
            var dataType = dataTypeValue.Split(':')[0];
            var nameSpace = rootNode.Attributes()
                .Where(x => x.Name.Equals(XNamespace.Xmlns + dataType))
                .FirstOrDefault().Value.Replace(XamlConstants.ClrNamespace, "");
            return string.Join(".", nameSpace, dataTypeValue.Split(':')[1]);
        }
        public static bool HasParent(this XElement element, string parentName)
        {
            XElement parent = element.Parent;

            while (parent != null)
            {
                if (parent.Name.LocalName == parentName)
                {
                    return true;
                }

                parent = parent.Parent;
            }

            return false;
        }

        // Recursive method to check for the specific parent anywhere in the ancestor chain
        private static bool HasAncestor(XElement element, string parentName)
        {
            XElement parent = element.Parent;

            if (parent == null)
            {
                return false;
            }

            if (parent.Name.LocalName == parentName)
            {
                return true;
            }

            return HasAncestor(parent, parentName);
        }
    }
}

