using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools
{
    public class RandomList<T>
    {
        private readonly List<KeyValuePair<float, T>> chanceMap;

        public RandomList(List<KeyValuePair<float, T>> chanceMap)
        {
            float factor = 1 / chanceMap.Sum(x => x.Key);
            this.chanceMap = chanceMap.
                Select(x => new KeyValuePair<float, T>(x.Key * factor, x.Value)).
                OrderBy(x => x.Key).ToList();
        }

        public T GetValue()
        {
            float currentChance = Random.Range(0, 1f);
            float accumulatedChance = 0;

            foreach (var kvp in chanceMap)
            {
                accumulatedChance += kvp.Key;

                if (accumulatedChance >= currentChance)
                {
                    return kvp.Value;
                }
            }

            return chanceMap.Last().Value;
        }
    }
}
