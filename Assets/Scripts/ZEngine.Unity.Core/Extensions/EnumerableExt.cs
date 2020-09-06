using System;
using System.Collections.Generic;

namespace ZEngine.Unity.Core.Extensions
{
    public static class EnumerableExt
    {
        public static void ForEach<T>(
            this IEnumerable<T> source,
            Action<T> action)
        {
            foreach (T element in source) 
                action(element);
        }
    }
}