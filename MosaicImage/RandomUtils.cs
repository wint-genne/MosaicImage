using System;
using System.Collections.Generic;
using System.Linq;

namespace MosaicImage
{
    internal class RandomUtils
    {
        public static T GetRandomLowest<T>(IEnumerable<T> enumerable, Func<T, double> valFunc)
        {
            return GetRandomHighest(enumerable, i => 1/valFunc(i));
        }

        private static T GetRandomHighest<T>(IEnumerable<T> enumerable, Func<T, double> valFunc)
        {
            var rand = new Random();
            var array = enumerable as T[] ?? enumerable.ToArray();
            var total = array.Sum(valFunc);
            var randInt = rand.NextDouble() * total;
            var currentVal = 0.0;
            foreach (var v in array)
            {
                currentVal += valFunc(v);
                if (randInt < currentVal)
                    return v;
            }
            throw new InvalidOperationException("No random found");
        }
    }
}