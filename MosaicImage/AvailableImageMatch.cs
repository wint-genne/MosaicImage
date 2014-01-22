using System;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class AvailableImageMatch
    {
        public AvailableImage AvailableImage { get; set; }
        private readonly SourcePixelBlock _sourcePixelBlock;

        public float Difference { get; private set; }

        public AvailableImageMatch(AvailableImage availableImage, SourcePixelBlock sourcePixelBlock)
        {
            AvailableImage = availableImage;
            _sourcePixelBlock = sourcePixelBlock;

            CalculateDifference();
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

        private float GetBrightnessDifference()
        {
            var brightnessDifference = GetBrightness(_sourcePixelBlock.OriginalAverageColor) - GetBrightness(AvailableImage.AverageColor);
            return brightnessDifference;
        }

        private float GetBrightness(Color color)
        {
            return new[]{color.R, color.G, color.B}.Average(x => (float)x);
        }

        public TargetPixelGenerator GetTargetPixelGenerator()
        {
            return new TargetPixelGenerator(AvailableImage, _sourcePixelBlock);
        }
    }
}