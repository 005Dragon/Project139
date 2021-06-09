using System;

namespace BattleCore
{
    public class DefaultRandom : IRandom
    {
        private readonly Random _random;

        public DefaultRandom()
        {
            _random = new Random();
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