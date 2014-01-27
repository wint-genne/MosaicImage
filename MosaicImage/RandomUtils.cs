using System;
using System.Collections.Generic;
using System.Linq;

namespace MosaicImage
{
    public class RandomUtils
    {
        public static T GetRandomLowest<T>(IEnumerable<T> enumerable, Func<T, double> valFunc, Random random, int min)
        {
            return GetRandomHighest(enumerable, i => 1/(valFunc(i)-min+1), random);
        }

        private static T GetRandomHighest<T>(IEnumerable<T> enumerable, Func<T, double> valFunc, Random rand)
        {
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