﻿
using Attributes.WebAttributes.HttpMethod;
using Generators.Base.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Crawler.Models
{
    public class ClassWithMethods
    {
        public ClassWithMethods(INamedTypeSymbol c)
        {
            Class = c;

            Methods = c.GetMethodsWithAttributesIncludingBaseTypes().ToList();
        }

        public INamedTypeSymbol Class { get; set; }
        private List<IMethodSymbol> Methods { get; set; }

        public IMethodSymbol MethodFromAttribute<TAttribute>() where TAttribute : BaseHttpAttribute
        {
            foreach (var method in Methods)
            {
                if (method.HasAttribute(typeof(TAttribute).Name))
                {
                    return method;
                }
            }

            return null;
        }
    }
}