﻿using System;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class MatchingImage
    {
        public AvailableImage AvailableImage { get; set; }
        private PixelBlock _pixelBlock;

        public MatchingImage(AvailableImage availableImage, PixelBlock pixelBlock)
        {
            AvailableImage = availableImage;
            _pixelBlock = pixelBlock;

            CalculateDifference(pixelBlock);
        }

        private void CalculateDifference(PixelBlock pixelBlock)
        {
            Difference = GetDifferenceAtPixel(pixelBlock.OriginalAverageColor);
        }

        private float GetDifferenceAtPixel(Color originalAverageColor)
        {
            return CompareColors(AvailableImage.Average, originalAverageColor);
        }

        private float CompareColors(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }

        public float Difference { get; private set; }

        public Color ReadPixel(Pixel pixel)
        {
            return AdjustBrightness(AvailableImage.GetAverageAtPixel(pixel.Subtract(_pixelBlock.GetPixels().First())));
        }

        private Color AdjustBrightness(Color color)
        {
            int b = (int)GetBrightnessDifference();
            return Color.FromArgb(ColorValue(color.R + b), ColorValue(color.G + b), ColorValue(color.B + b));
        }

        private int ColorValue(int p0)
        {
            return Math.Min(255, Math.Max(0, p0));
        }

        private float GetBrightnessDifference()
        {
            var brightnessDifference = GetBrightness(_pixelBlock.OriginalAverageColor) - GetBrightness(AvailableImage.Average);
            return brightnessDifference;
        }

        private float GetBrightness(Color color)
        {
            return new[]{color.R, color.G, color.B}.Average(x => (float)x);
        }
    }
}