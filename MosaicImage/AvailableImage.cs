using System.Drawing;
using System.Linq;

namespace MosaicImage
{
    public class AvailableImage
    {
        public int TargetBlockSize { get; set; }
        private Bitmap _bitmap;

        public AvailableImage(int targetBlockSize, Bitmap bitmap)
        {
            TargetBlockSize = targetBlockSize;
            using (var original = bitmap)
            {
                int newWidth = targetBlockSize;
                int newHeight = targetBlockSize;
                if (original.Width > original.Height)
                    newWidth = targetBlockSize*original.Width/original.Height;
                else
                    newHeight = targetBlockSize*original.Height/original.Width;
                _bitmap = new Bitmap(original, newWidth, newHeight);
            }
            AverageColor = ImageUtils.GetAverageColor(ImageUtils.GetPixels(0, 0, targetBlockSize).Select(p => _bitmap.GetPixel(p.X, p.Y)).ToArray());
        }

        public Color AverageColor { get; private set; }

        public Color GetAverageAtPixel(Pixel pixel)
        {
            return _bitmap.GetPixel(pixel.X, pixel.Y);
        }
    }
}