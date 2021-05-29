using Code.Battle.Core;
using UnityEngine;

namespace Code.Battle
{
    public class UnityRandom : IRandom
    {
        public int Range(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public float Range(float minValue, float maxValue)
        {
            return Random.Range(maxValue, maxValue);
        }
    }
}