using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceImage = args.Length > 0 ? args[0] : @"c:\temp\siri.png";
            int blockSize = args.Length > 1 ? int.Parse(args[1]) : 10;
            string availableImagesDir = args.Length > 2 ? args[2] : @"c:\temp\mosaicimages";
            var blocks = ReadBlocks(sourceImage, blockSize);
            var images = ReadAvailableImages(availableImagesDir, blockSize).ToArray();
            var availableImages = images.ToList();
            var targetImage = CreateTargetImage(blocks);
            Console.WriteLine("Creating...");
            int current = 0;
            int total = blocks.Blocks.Count();
            foreach (var pixelBlock in blocks.Blocks)
            {
                current++;
                int percentage = current * 100 / total;
                Console.Write("\r" + (percentage + "%").PadRight(10));
                var matchingImage = FindBestMatchingImage(pixelBlock, availableImages);
                if (availableImages.Count < 10) availableImages = images.ToList();
                UpdateBlock(targetImage, pixelBlock, matchingImage);
            }
            targetImage.Save("c:\\temp\\siri_mosaic.png");
        }

        private static void UpdateBlock(Bitmap targetImage, PixelBlock pixelBlock, MatchingImage matchingImage)
        {
            foreach (var pixel in pixelBlock.GetPixels())
            {
                targetImage.SetPixel(pixel.X, pixel.Y, matchingImage.ReadPixel(pixel));
            }
        }

        private static Bitmap CreateTargetImage(ImageBlocks imageBlocks)
        {
            return new Bitmap(imageBlocks.NumBlocksX * imageBlocks.BlockSize, imageBlocks.NumBlocksY * imageBlocks.BlockSize);
        }

        private static MatchingImage FindBestMatchingImage(PixelBlock pixelBlock, List<AvailableImage> availableImages)
        {
            var findBestMatchingImage = availableImages.Select(i => new MatchingImage(i, pixelBlock)).OrderBy(m => m.Difference).First();
            availableImages.Remove(findBestMatchingImage.AvailableImage);
            return findBestMatchingImage;
        }

        private static IEnumerable<AvailableImage> ReadAvailableImages(string sourceDir, int blockSize)
        {
            foreach (var imageFile in Directory.EnumerateFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                Console.WriteLine("Reading " + imageFile);
                yield return new AvailableImage(imageFile, blockSize);
            }
        }

        private static ImageBlocks ReadBlocks(string sourceImage, int blockSize)
        {
            return new ImageBlocks(sourceImage, blockSize);
        }
    }

    internal class ImageBlocks
    {
        public List<PixelBlock> Blocks { get; set; }

        public int NumBlocksX { get; private set; }
        public int NumBlocksY { get; private set; }

        public int BlockSize { get; set; }

        public ImageBlocks(string sourceImage, int blockSize)
        {
            Bitmap b = new Bitmap(sourceImage);
            Console.WriteLine("Reading blocks...");
            NumBlocksX = (int)Math.Floor((double)b.Width / blockSize);
            NumBlocksY = (int)Math.Floor((double)b.Height / blockSize);
            BlockSize = blockSize;
            Blocks = new List<PixelBlock>();
            for (int x = 0; x < NumBlocksX; x++)
            {
                Console.Write("\r" + (x * 100 / NumBlocksX) + "%    ");
                for (int y = 0; y < NumBlocksY; y++)
                {
                    Blocks.Add(new PixelBlock(b, x, y, blockSize));
                }
            }
        }
    }
}
