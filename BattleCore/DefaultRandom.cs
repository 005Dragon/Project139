using System;

namespace BattleCore
{
    public class DefaultRandom : IRandom
    {
        private readonly Random _random;

        public DefaultRandom(Random random)
        {
            _random = random;
        }

        public int Range(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public float Range(float minValue, float maxValue)
        {
            float magnitude = maxValue - minValue;

            return minValue + (magnitude * (float)_random.NextDouble());
        }
    }
}