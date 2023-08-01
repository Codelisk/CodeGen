using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Generator.Constants
{
    public static class XamlConstants
    {
        public const string ClrNamespace = "clr-namespace:";
        public const string BindingFinderPattern = @"{Binding\s+((Path=)?.*?)(,\s*(.*?))?\}";
        public const string DataTemplateNodeName = "DataTemplate";
        public const string DataTypeAttributeName = "DataType";
        public const string BindingName = "{Binding ";
        public const string BindingBindingContextPrefix = "BindingContext.";
        public const string BindingPathPrefix = "Path=";
        public const string MauiControlsNamespace = "Microsoft.Maui.Controls";
        public const string FakeMauiControlsNamespace = "http://schemas.microsoft.com/dotnet/2021/maui";
        public const string XClassName = "Class";
    }
}
