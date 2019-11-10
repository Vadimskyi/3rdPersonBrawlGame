using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Utils
{
    public static class EnumerableExtend
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> callback)
        {
            foreach (var item in source)
            {
                callback?.Invoke(item);
            }

            return source;
        }
    }
}
