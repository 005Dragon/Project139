using System;
using System.Collections.Generic;

namespace Code.Utils
{
    public static class EnumExtensions
    {
        public static PlayerId GetAnother(this PlayerId playerId)
        {
            switch (playerId)
            {
                case PlayerId.Left: return PlayerId.Right;
                case PlayerId.Right: return PlayerId.Left;
                default: throw new ArgumentOutOfRangeException(nameof(playerId), playerId, null);
            }
        }

        public static IEnumerable<T> GetAllValues<T>()
            where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            
            foreach (object value in values)
            {
                yield return (T) value;
            }
        }
    }
}