using Attributes.MauiAttributes;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Generator.Extensions
{
    public static class ContextExtensions
    {
        public static INamedTypeSymbol BaseViewModel(this GeneratorExecutionContext context)
        {
            return context.GetClassesWithAttribute(nameof(BaseViewModelAttribute)).First();
        }
        public static IEnumerable<AdditionalText> GetXamlFiles(this GeneratorExecutionContext context)
        {
            // Get all the additional files from the context
            IEnumerable<AdditionalText> additionalFiles = context.AdditionalFiles;

            // Filter the XAML files from the additional files
            List<string> xamlFileContents = new List<string>();
            foreach (var additionalFile in additionalFiles)
            {
                // Check if the file has a .xaml extension
                if (additionalFile.Path.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase))
                {
                    // Read the XAML content from the additional file
                    string xamlContent = additionalFile.GetText()?.ToString();
                    if (!string.IsNullOrEmpty(xamlContent))
                    {
                        // Store the XAML content in the list
                        xamlFileContents.Add(xamlContent);
                    }
                }
            }
            return additionalFiles;
        }
    }
}
