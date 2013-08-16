using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class PixelBlock
    {
        private readonly int _x;
        private readonly int _y;

        public PixelBlock(Bitmap sourceBitmap, int x, int y, int blockSize)
        {
            _x = x;
            _y = y;

            CalculateOriginalAverageColor(sourceBitmap, blockSize);
        }

        public override string ToString()
        {
            return _x + "," + _y;
        }

        private void CalculateOriginalAverageColor(Bitmap bitmap, int blockSize)
        {
            OriginalAverageColor = ImageUtils.GetAverageColor(GetPixels(blockSize).Select(p => bitmap.GetPixel(p.X, p.Y)).ToArray());
        }

        public Color OriginalAverageColor { get; set; }

        public IEnumerable<Pixel> GetPixels(int blockSize)
        {
            return ImageUtils.GetPixels(_x * blockSize, _y * blockSize, blockSize);
        }
    }
}