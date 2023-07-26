using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Generators.Base;
using System.Xml.Linq;

namespace Controller.Generator
{
    public static class Extensions
    {
        private static string ReplaceDtoSuffix(this INamedTypeSymbol dto)
        {
            return dto.Name.Replace("Dto", "");
        }
        public static string ControllerNameFromDto(this INamedTypeSymbol dto)
        {
            return $"{dto.ReplaceDtoSuffix()}Controller";
        }
        public static string PostAttribute(this INamedTypeSymbol dto, bool plural = false)
        {
            return AttributeWithName(Constants.HttpPostAttribute, $"Save{dto.ReplaceDtoSuffix()}", plural);
        }
        public static string GetAttribute(this INamedTypeSymbol dto, bool plural = false)
        {
            return AttributeWithName(Constants.HttpGetAttribute, $"Get{dto.ReplaceDtoSuffix()}", plural);
        }
        public static string DeleteAttribute(this INamedTypeSymbol dto, bool plural = false)
        {
            return AttributeWithName(Constants.HttpDeleteAttribute, $"Delete{dto.ReplaceDtoSuffix()}", plural);
        }
        private static string AttributeWithName(string attribute, string nameValue, bool pluralName = false)
        {
            nameValue = pluralName ? nameValue.Pluralize() : nameValue;
            return $"{attribute}(\"{nameValue}\")";
        }

        public static string GetMethodName(this INamedTypeSymbol dto)
        {
            return $"Get{dto.ReplaceDtoSuffix()}";
        }
        public static string GetAllMethodName(this INamedTypeSymbol dto)
        {
            return $"GetAll{dto.ReplaceDtoSuffix().Pluralize()}";
        }
        public static string SaveMethodName(this INamedTypeSymbol dto)
        {
            return $"Save{dto.ReplaceDtoSuffix()}";
        }
        public static string DeleteMethodName(this INamedTypeSymbol dto)
        {
            return $"Delete{dto.ReplaceDtoSuffix()}";
        }
    }
}
