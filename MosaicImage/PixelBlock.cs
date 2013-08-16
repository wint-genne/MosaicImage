using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class PixelBlock
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _blockSize;

        public PixelBlock(Bitmap bitmap, int x, int y, int blockSize)
        {
            _x = x * blockSize;
            _y = y * blockSize;
            _blockSize = blockSize;

            CalculateOriginalAverageColor(bitmap);
        }

        public override string ToString()
        {
            return _x + "," + _y;
        }

        private void CalculateOriginalAverageColor(Bitmap bitmap)
        {
            OriginalAverageColor = ImageUtils.GetAverageColor(GetPixels().Select(p => bitmap.GetPixel(p.X, p.Y)).ToArray());
        }

        public Color OriginalAverageColor { get; set; }

        public IEnumerable<Pixel> GetPixels()
        {
            return ImageUtils.GetPixels(_x, _y, _blockSize);
        }
    }
}