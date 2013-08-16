namespace MosaicImage
{
    internal class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Pixel Subtract(Pixel firstPixel)
        {
            return new Pixel(X - firstPixel.X, Y - firstPixel.Y);
        }
    }
}