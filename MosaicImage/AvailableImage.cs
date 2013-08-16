using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    internal class AvailableImage
    {
        private Bitmap _bitmap;

        public AvailableImage(string imageFile, int blockSize)
        {
            using (var original = new Bitmap(imageFile))
            {
                int newWidth = blockSize;
                int newHeight = blockSize;
                if (original.Width > original.Height)
                    newWidth = blockSize*original.Width/original.Height;
                else
                    newHeight = blockSize*original.Height/original.Width;
                _bitmap = new Bitmap(original, newWidth, newHeight);
            }
            Average = ImageUtils.GetAverageColor(ImageUtils.GetPixels(0, 0, blockSize).Select(p => _bitmap.GetPixel(p.X, p.Y)).ToArray());
        }

        public Color Average { get; private set; }

        public Color GetAverageAtPixel(Pixel pixel)
        {
            return _bitmap.GetPixel(pixel.X, pixel.Y);
        }
    }
}