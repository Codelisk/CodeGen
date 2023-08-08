﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoFilterer.Extensions;

public static class NullableExtensions
{
    public static bool IsNullable(this Type type)
    {
        return type.Name == typeof(Nullable<>).Name;
    }

    public static T? MakeNullable<T>(this T value)
        where T : struct
    {
        var genericNullableType = typeof(Nullable<>).MakeGenericType(typeof(T));
        return Activator.CreateInstance(genericNullableType, args: new object[] { value }) as T?;
    }

    public static Type AsNonNullable(this Type type)
    {
        if (type.IsNullable())
        {
            return type.GetGenericArguments()[0];
        }
        return type;
    }
}
