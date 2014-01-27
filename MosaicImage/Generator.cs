using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MosaicImage
{
    public class Generator
    {
        private const int SourceMaxWidthOrHeight = 760;

        public static void GenerateMosaicImage(string sourceImagePath, int sourceBlockSize, int blockTargetSize,
                                               string availableImagesDir, string targetImagePath)
        {
            var originalAvailableImages = ReadAvailableImages(availableImagesDir, blockTargetSize).ToArray();
            GenerateMosaicImage(sourceImagePath, sourceBlockSize, blockTargetSize, targetImagePath, originalAvailableImages);
        }

        public static void GenerateMosaicImage(string sourceImagePath, int sourceBlockSize, int blockTargetSize,
                                                string targetImagePath, AvailableImage[] originalAvailableImages)
        {
            Console.WriteLine(sourceImagePath);
            var sourceBitmap = new Bitmap(sourceImagePath);
            var resizedSource = new Bitmap(sourceBitmap, GetSourceFixedSize(sourceBitmap));
            var blocks = new SourceImage(sourceBlockSize, resizedSource);
            var targetImage = GenerateTargetImage(blocks, originalAvailableImages, blockTargetSize, targetImagePath, sourceBitmap);
            targetImage.SetResolution(220, 220);
            targetImage.Save(targetImagePath, sourceBitmap.RawFormat);
        }

        private static Size GetSourceFixedSize(Image sourceBitmap)
        {
            return sourceBitmap.Width > sourceBitmap.Height 
                ? new Size((int)((float)sourceBitmap.Width / sourceBitmap.Height * SourceMaxWidthOrHeight), SourceMaxWidthOrHeight) 
                : new Size(SourceMaxWidthOrHeight, (int)((float)sourceBitmap.Height / sourceBitmap.Width * SourceMaxWidthOrHeight));
        }

        public static Bitmap GenerateTargetImage(SourceImage sourceImage, AvailableImage[] availableImages, int targetBlockSize, string targetImagePath, Bitmap sourceBitmap)
        {
            var availableImagesCopy = availableImages.ToList();
            var targetImage = CreateTargetImage(sourceImage, targetBlockSize);
            Console.WriteLine("Creating...");
            var current = 0;
            var total = sourceImage.Blocks.Count();
            var prevPercentage = 0;
            foreach (var sourcePixelBlock in sourceImage.Blocks)
            {
                current++;
                int percentage = current*100/total;
                if (prevPercentage != percentage)
                {
                    Console.Write("\r" + (percentage + "%").PadRight(10));
                    prevPercentage = percentage;
                    targetImage.Save(targetImagePath, sourceBitmap.RawFormat);
                }
                var matchingImage = FindBestMatchingImage(sourcePixelBlock, availableImagesCopy);
                if (availableImagesCopy.Count < 10) availableImagesCopy = availableImages.ToList();
                UpdateBlock(targetImage, sourcePixelBlock, new TargetPixelGenerator(matchingImage, sourcePixelBlock));
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

        private static AvailableImage FindBestMatchingImage(SourcePixelBlock sourcePixelBlock, ICollection<AvailableImage> availableImages)
        {
            return new AvailableImageFinder(sourcePixelBlock.OriginalAverageColor).Find(availableImages);
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                    {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static IEnumerable<AvailableImage> ReadAvailableImages(string sourceDir, int targetBlockSize)
        {
            foreach (var imageFile in Directory.EnumerateFiles(sourceDir, "*.jpg", SearchOption.AllDirectories))
            {
                Console.WriteLine("Reading " + imageFile);
                AvailableImage availableImage = null;
                try
                {
                    availableImage = new AvailableImage(targetBlockSize, MakeGrayscale3(new Bitmap(imageFile)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (availableImage != null)
                    yield return availableImage;
            }
        }

        public static int DpiToBlockSize(int dpi, int targetWidthInCm, int sourceBlockSize)
        {
            double pixelsPerCm = dpi / 2.54;
            var targetWidthInPixels = targetWidthInCm * pixelsPerCm;
            double numBlocks = (double)SourceMaxWidthOrHeight / sourceBlockSize;
            return (int)(targetWidthInPixels / numBlocks);
        }
    }
}