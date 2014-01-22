using System;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class AvailableImageMatch
    {
        public AvailableImage AvailableImage { get; set; }
        private readonly SourcePixelBlock _sourcePixelBlock;
        private readonly Tuple<int, int, int> _colorDifference;

        public float Difference { get; private set; }

        public AvailableImageMatch(AvailableImage availableImage, SourcePixelBlock sourcePixelBlock)
        {
            AvailableImage = availableImage;
            _sourcePixelBlock = sourcePixelBlock;

            CalculateDifference();
            _colorDifference = Tuple.Create(
                _sourcePixelBlock.OriginalAverageColor.R - AvailableImage.AverageColor.R,
                _sourcePixelBlock.OriginalAverageColor.G - AvailableImage.AverageColor.G,
                _sourcePixelBlock.OriginalAverageColor.B - AvailableImage.AverageColor.B
                );
            //_colorDifference = Tuple.Create(0, 0, 0);
        }

        private void CalculateDifference()
        {
            Difference = GetDifferenceAtPixel(_sourcePixelBlock.OriginalAverageColor);
        }

        private float GetDifferenceAtPixel(Color sourceImageAverageColor)
        {
            return CompareColors(AvailableImage.AverageColor, sourceImageAverageColor);
        }

        private static float CompareColors(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }

        public Color ReadPixel(Pixel pixel)
        {
            var averageColor = AvailableImage.GetAverageAtPixel(pixel.Subtract(_sourcePixelBlock.GetPixels(AvailableImage.TargetBlockSize).First()));
            return ApplyColorDifference(averageColor);
        }

        private Color ApplyColorDifference(Color color)
        {
            return Color.FromArgb(ColorValue(color.R + _colorDifference.Item1), ColorValue(color.G + _colorDifference.Item2), ColorValue(color.B + _colorDifference.Item3));
        }

        private int ColorValue(int p0)
        {
            return Math.Min(255, Math.Max(0, p0));
        }

        private float GetBrightnessDifference()
        {
            var brightnessDifference = GetBrightness(_sourcePixelBlock.OriginalAverageColor) - GetBrightness(AvailableImage.AverageColor);
            return brightnessDifference;
        }

        private float GetBrightness(Color color)
        {
            return new[]{color.R, color.G, color.B}.Average(x => (float)x);
        }
    }
}