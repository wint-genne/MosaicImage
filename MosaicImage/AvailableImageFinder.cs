using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class AvailableImageFinder
    {
        private static Dictionary<int, AvailableImage> _cache = new Dictionary<int, AvailableImage>(); 
        private Color _originalAverageColor;

        public AvailableImageFinder(Color originalAverageColor)
        {
            _originalAverageColor = originalAverageColor;
        }

        public AvailableImage Find(ICollection<AvailableImage> availableImages)
        {
            var findBestMatchingImage = RandomUtils.GetRandomLowest(availableImages, m => CompareColors(m), new Random(), 0);
            availableImages.Remove(findBestMatchingImage);
            return findBestMatchingImage;
        }

        public int CompareColors(AvailableImage a)
        {
            return Math.Abs(a.AverageColor.R - _originalAverageColor.R) + Math.Abs(a.AverageColor.G - _originalAverageColor.G) + Math.Abs(a.AverageColor.B - _originalAverageColor.B);
        }
    }
}