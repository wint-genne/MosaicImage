using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MosaicImage
{
    public class Generator
    {
        public static void GenerateMosaicImage(string sourceImage, int blockSize, int blockTargetSize,
                                               string availableImagesDir)
        {
            var blocks = new SourceImage(blockSize, new Bitmap(sourceImage));
            var originalAvailableImages = ReadAvailableImages(availableImagesDir, blockTargetSize).ToArray();
            var targetImage = GenerateTargetImage(blocks, originalAvailableImages, blockTargetSize);
            targetImage.Save("c:\\temp\\siri_mosaic5.png");
        }

        public static Bitmap GenerateTargetImage(SourceImage sourceImage, AvailableImage[] availableImages, int targetBlockSize)
        {
            var availableImagesCopy = availableImages.ToList();
            var targetImage = CreateTargetImage(sourceImage, targetBlockSize);
            Console.WriteLine("Creating...");
            var current = 0;
            var total = sourceImage.Blocks.Count();
            foreach (var sourcePixelBlock in sourceImage.Blocks)
            {
                current++;
                int percentage = current*100/total;
                Console.Write("\r" + (percentage + "%").PadRight(10));
                var matchingImage = FindBestMatchingImage(sourcePixelBlock, availableImagesCopy);
                if (availableImagesCopy.Count < 10) availableImagesCopy = availableImages.ToList();
                UpdateBlock(targetImage, sourcePixelBlock, matchingImage.GetTargetPixelGenerator());
            }
            return targetImage;
        }

        private static void UpdateBlock(Bitmap targetImage, SourcePixelBlock sourcePixelBlock, TargetPixelGenerator targetPixelGenerator)
        {
            foreach (var pixel in sourcePixelBlock.GetPixels(targetPixelGenerator.TargetBlockSize))
            {
                targetImage.SetPixel(pixel.X, pixel.Y, targetPixelGenerator.GetTargetPixel(pixel));
            }
        }

        private static Bitmap CreateTargetImage(SourceImage sourceImage, int targetBlockSize)
        {
            return new Bitmap(sourceImage.NumBlocksX * targetBlockSize, sourceImage.NumBlocksY * targetBlockSize);
        }

        private static AvailableImageMatch FindBestMatchingImage(SourcePixelBlock sourcePixelBlock, ICollection<AvailableImage> availableImages)
        {
            var findBestMatchingImage = availableImages.Select(i => new AvailableImageMatch(i, sourcePixelBlock)).OrderBy(m => m.Difference).First();
            availableImages.Remove(findBestMatchingImage.AvailableImage);
            return findBestMatchingImage;
        }

        private static IEnumerable<AvailableImage> ReadAvailableImages(string sourceDir, int targetBlockSize)
        {
            foreach (var imageFile in Directory.EnumerateFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                Console.WriteLine("Reading " + imageFile);
                yield return new AvailableImage(targetBlockSize, new Bitmap(imageFile));
            }
        }
    }
}