using CodeGenHelpers;
using CodeGenHelpers.Internals;
using Generator.Foundation.Generators.Base;
using Generators.Base.Extensions;
using Maui.Generator.Constants;
using Maui.Generator.Extensions;
using Maui.Generator.Extensions.Xaml;
using Maui.Generator.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Maui.Generator.Generators.Codebuilder
{
    public class BaseViewModelBuilder : BaseCodeBuilder
    {
        public BaseViewModelBuilder(string codeBuilderNamespace) : base(codeBuilderNamespace)
        {
        }
        public override List<CodeBuilder> Get(GeneratorExecutionContext context, List<CodeBuilder> codeBuilders = null)
        {
            var baseVm = context.BaseViewModel();
            List<CodeBuilder> result = new();
            var classes = context.GetAllClasses(string.Empty).ToList();
            var files = context.GetXamlFiles();

            foreach (var file in files)
            {
                var xamlExtract = File.ReadAllText(file.Path).GetAllBindings();
                CodeBuilder codeBuilder = CodeBuilder.Create(context.Compilation.AssemblyName);
                var myClass = codeBuilder.AddClass(xamlExtract.ClassName + ViewModelConstants.ViewModelSuffix).WithAccessModifier(Accessibility.Public).SetBaseClass(baseVm);
                foreach (var item in xamlExtract.Bindings)
                {
                    var propertyType = GetBindingTypeFromControl(classes, item);
                    myClass = AddBindingProperty(myClass, item, propertyType);
                }

                result.Add(codeBuilder);
            }

            return result;
        }
        private ClassBuilder AddBindingProperty(ClassBuilder classBuilder, BindingElement bindingElement, ITypeSymbol propertyType)
        {
            string propertyTypeName = propertyType.ToDisplayString();

            bool isList = false;
            if (bindingElement.DataType is not null)
            {
                if (propertyType.IsListType())
                {
                    isList = true;
                    propertyTypeName = $"List<{bindingElement.DataType}>";
                }
                else
                {
                    propertyTypeName = bindingElement.DataType;
                }
            }

            var propertyBuilder = classBuilder
                .AddProperty(bindingElement.Value, Accessibility.Public)
                .SetType(propertyTypeName);

            if (!isList)
            {
                propertyBuilder.AddAttribute(ViewModelConstants.BindingPropertyAttribute);
            }

            propertyBuilder.UseAutoProps();

            return classBuilder;
        }
        private ITypeSymbol GetBindingTypeFromControl(List<INamedTypeSymbol> allClasses, BindingElement bindingElement)
        {
            var element = allClasses.FirstOrDefault(x => x.Name == bindingElement.ElementName && x.ContainingNamespace.ToDisplayString().Equals(bindingElement.ElementNamespace));
            var property = element?.GetFirstPropertyByName(bindingElement.PropertyName);

            //When the property is not found, we assume that the property is a custom type
            if (property is null)
            {
                return allClasses.FirstOrDefault(x => x.Name == bindingElement.PropertyName);
            }

            return property.Type;
        }
        private XamlExtract GetAllBindings(string xamlContent)
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

    }
}
