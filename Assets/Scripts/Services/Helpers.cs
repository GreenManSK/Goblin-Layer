using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Services
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();

        public static T GetRandom<T>(IReadOnlyList<T> array)
        {
            var index = Random.Next(0, array.Count);
            return array[index];
        }

        public static Color ChangeAlpha(Color c, float alpha)
        {
            c.a = alpha;
            return c;
        }
    }
}