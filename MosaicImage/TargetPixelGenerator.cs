using System;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class TargetPixelGenerator
    {
        private readonly AvailableImage _availableImage;
        private readonly SourcePixelBlock _sourcePixelBlock;
        public int TargetBlockSize { get { return _availableImage.TargetBlockSize; } }

        public TargetPixelGenerator(AvailableImage availableImage, SourcePixelBlock sourcePixelBlock)
        {
            _availableImage = availableImage;
            _sourcePixelBlock = sourcePixelBlock;
        }

        public Color GetTargetPixel(Pixel pixel)
        {
            var averageColor = _availableImage.GetPixel(pixel.Subtract(_sourcePixelBlock.GetPixels(TargetBlockSize).First()));
            return ApplyColorDifference(averageColor);
        }

        private Color ApplyColorDifference(Color color)
        {
            var sourceAverageColor = _sourcePixelBlock.OriginalAverageColor;
            var availableAverageColor = _availableImage.AverageColor;
            return Color.FromArgb(
                ImageUtils.ColorValue(color.R + sourceAverageColor.R - availableAverageColor.R),
                ImageUtils.ColorValue(color.G + sourceAverageColor.G - availableAverageColor.G),
                ImageUtils.ColorValue(color.B + sourceAverageColor.B - availableAverageColor.B));
        }
    }
}