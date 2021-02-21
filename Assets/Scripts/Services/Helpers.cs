using System;
using System.Collections.Generic;

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
    }
}