using Maui.Generator.Constants;
using Maui.Generator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Maui.Generator.Extensions.Xaml
{
    public static class XamlContentExtensions
    {
        private static (string @namespace, string filename) ExtractNamespaceAndFileName(string inputString)
        {
            int lastDotIndex = inputString.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                string @namespace = inputString.Substring(0, lastDotIndex);
                string filename = inputString.Substring(lastDotIndex + 1);
                return (@namespace, filename);
            }
            else
            {
                // If there is no dot in the string, consider the whole string as the filename.
                return (string.Empty, inputString);
            }
        }
        public static  XamlExtract GetAllBindings(this string xamlContent)
        {
            var result = new XamlExtract();
            var xDocument = XDocument.Parse(xamlContent);
            var descendants = xDocument.Descendants().ToList();
            string pattern = @"{Binding\s+((Path=)?.*?)(,\s*(.*?))?\}";

            var rootNode = descendants.First();
            foreach (var attribute in rootNode.Attributes())
            {
                if (attribute.Name.LocalName.Equals(XamlConstants.XClassName))
                {
                    var nameSpaceAndFile = ExtractNamespaceAndFileName(attribute.Value);
                    result.ClassName = nameSpaceAndFile.filename;
                    result.ClassNamespace = nameSpaceAndFile.@namespace;
                }
            }

            string? dataType = null;
            XElement? dataTypeElement = null;
            foreach (var descendant in descendants.Skip(0))
            {
                if (dataTypeElement is not null)
                {
                    if (!descendant.HasParent(dataTypeElement.Name.LocalName))
                    {
                        dataTypeElement = null;
                        dataType = null;
                    }
                }
                if (descendant.HasParent(XamlConstants.DataTemplateNodeName))
                {
                    continue;
                }
                var attributes = descendant.Attributes().ToList();
                if (attributes.Count > 0)
                {
                    var dataTypeAttribute = attributes.FirstOrDefault(x => x.Name.LocalName.Equals(XamlConstants.DataTypeAttributeName));
                    if (dataTypeAttribute is not null)
                    {
                        dataTypeElement = dataTypeAttribute.Parent;
                        dataType = rootNode.GetDataType(dataTypeAttribute.Value);
                    }

                    foreach (var attribute in attributes)
                    {
                        if (attribute.Value.Contains(XamlConstants.BindingName))
                        {
                            var propertyName = attribute.Name.LocalName;
                            var elementName = descendant.Name.LocalName;
                            MatchCollection bindingMatches = Regex.Matches(attribute.Value, XamlConstants.BindingFinderPattern);
                            var value = bindingMatches[0].Groups[1].Value.Replace("'", "").Replace(XamlConstants.BindingPathPrefix, "").Replace(XamlConstants.BindingBindingContextPrefix, "");
                            if (value.Contains("."))
                            {
                                value = value.Split('.').First();
                                propertyName = value;
                            }

                            result.Bindings.Add(new BindingElement
                            {
                                ElementName = elementName,
                                PropertyName = propertyName,
                                Value = value,
                                DataType = dataType,
                                ElementNamespace = descendant.Name.NamespaceName.Replace(XamlConstants.ClrNamespace, "").Replace(XamlConstants.FakeMauiControlsNamespace, XamlConstants.MauiControlsNamespace)
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}
