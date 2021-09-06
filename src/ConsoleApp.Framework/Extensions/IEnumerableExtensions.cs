using System.Collections.Generic;

namespace ConsoleApp.Framework.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static IEnumerable<T> Repeat<T>(this T value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return value;
            }
        }
    }
}
