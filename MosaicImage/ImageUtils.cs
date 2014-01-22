using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal static class ImageUtils
    {
        public static Color GetAverageColor(Color[] colors)
        {
            var red = (int)colors.Average(c => (double)c.R);
            var green = (int)colors.Average(c => (double)c.G);
            var blue = (int)colors.Average(c => (double)c.B);
            return Color.FromArgb(red, green, blue);
        }

        public static IEnumerable<Pixel> GetPixels(int _x, int _y, int blockSize)
        {
            for (int x = 0; x < blockSize; x++)
            {
                for (int y = 0; y < blockSize; y++)
                {
                    yield return new Pixel(x + _x, y + _y);
                }
            }
        }

        public static int CompareColors(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }

        public static int ColorValue(int val)
        {
            return Math.Min(255, Math.Max(0, val));
        }
    }
}