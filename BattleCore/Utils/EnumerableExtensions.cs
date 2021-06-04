using System.Collections.Generic;

namespace BattleCore.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToSingleElementEnumerable<T>(this T value)
        {
            yield return value;
        }
    }
}