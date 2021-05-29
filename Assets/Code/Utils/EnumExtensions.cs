using System;
using System.Collections.Generic;
using Code.Battle;

namespace Code.Utils
{
    public static class EnumExtensions
    {
        public static PlayerSide GetAnother(this PlayerSide playerSide)
        {
            switch (playerSide)
            {
                case PlayerSide.Left: return PlayerSide.Right;
                case PlayerSide.Right: return PlayerSide.Left;
                default: throw new ArgumentOutOfRangeException(nameof(playerSide), playerSide, null);
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