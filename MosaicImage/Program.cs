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
            var sourceImage = args.Length > 0 ? args[0] : @"c:\temp\mosaic\src\IMG_9446.jpg";
            int blockSize = args.Length > 1 ? int.Parse(args[1]) : 10;
            int blockTargetSize = 10;
            string availableImagesDir = args.Length > 2 ? args[2] : @"c:\temp\mosaic\siri";
            var blocks = ReadBlocks(sourceImage, blockSize, blockTargetSize);
            var images = ReadAvailableImages(availableImagesDir, blockTargetSize).ToArray();
            var availableImages = images.ToList();
            var targetImage = CreateTargetImage(blocks, blockTargetSize);
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
            targetImage.Save("c:\\temp\\siri_mosaic5.png");
        }

        private static void UpdateBlock(Bitmap targetImage, PixelBlock pixelBlock, MatchingImage matchingImage)
        {
            foreach (var pixel in pixelBlock.GetPixels(matchingImage.AvailableImage.TargetBlockSize))
            {
                targetImage.SetPixel(pixel.X, pixel.Y, matchingImage.ReadPixel(pixel));
            }
        }

        private static Bitmap CreateTargetImage(ImageBlocks imageBlocks, int targetBlockSize)
        {
            return new Bitmap(imageBlocks.NumBlocksX * targetBlockSize, imageBlocks.NumBlocksY * targetBlockSize);
        }

        private static MatchingImage FindBestMatchingImage(PixelBlock pixelBlock, List<AvailableImage> availableImages)
        {
            var findBestMatchingImage = availableImages.Select(i => new MatchingImage(i, pixelBlock)).OrderBy(m => m.Difference).First();
            availableImages.Remove(findBestMatchingImage.AvailableImage);
            return findBestMatchingImage;
        }

        private static IEnumerable<AvailableImage> ReadAvailableImages(string sourceDir, int targetBlockSize)
        {
            foreach (var imageFile in Directory.EnumerateFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                Console.WriteLine("Reading " + imageFile);
                yield return new AvailableImage(imageFile, targetBlockSize);
            }
        }

        private static ImageBlocks ReadBlocks(string sourceImage, int blockSize, int targetBlockSize)
        {
            return new ImageBlocks(sourceImage, blockSize);
        }
    }
}
