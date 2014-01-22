﻿using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MosaicImage.Tests
{
    [TestClass]
    public class MosaicImageTests
    {
        [TestMethod]
        public void TestTargetColor()
        {
            var sourceBitmap = new Bitmap(1, 1);
            var sourceColor = Color.Red;
            sourceBitmap.SetPixel(0, 0, sourceColor);
            var availableImageBitmap = new Bitmap(1, 1);
            availableImageBitmap.SetPixel(0, 0, Color.Purple);
            const int targetBlockSize = 1;
            var sourceImage = new SourceImage(1, sourceBitmap);
            var availableImages = new[]{ new AvailableImage(targetBlockSize, availableImageBitmap), };
            var targetImage = Generator.GenerateTargetImage(sourceImage, availableImages, targetBlockSize);

            Assert.AreEqual(1, targetImage.Width);
            Assert.AreEqual(1, targetImage.Height);
            var targetPixel = targetImage.GetPixel(0, 0);
            Assert.AreEqual(sourceColor.R, targetPixel.R);
            Assert.AreEqual(sourceColor.G, targetPixel.G);
            Assert.AreEqual(sourceColor.B, targetPixel.B);
        }
    }
}
