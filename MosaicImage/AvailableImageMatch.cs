using System;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class AvailableImageMatch
    {
        public AvailableImage AvailableImage { get; set; }
        private readonly SourcePixelBlock _sourcePixelBlock;

        public int Difference { get; private set; }

        public AvailableImageMatch(AvailableImage availableImage, SourcePixelBlock sourcePixelBlock)
        {
            AvailableImage = availableImage;
            _sourcePixelBlock = sourcePixelBlock;

            Difference = ImageUtils.CompareColors(AvailableImage.AverageColor, _sourcePixelBlock.OriginalAverageColor);
        }

        public TargetPixelGenerator GetTargetPixelGenerator()
        {
            return new TargetPixelGenerator(AvailableImage, _sourcePixelBlock);
        }
    }
}