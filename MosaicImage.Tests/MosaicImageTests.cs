using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

		[TestMethod]
        public void GetRandomLowestTestCase()
        {
			var vals = new[]{ 1, 2 };
			var hits = new Dictionary<int, int>{ { 1, 0 },{2, 0}};
		    var random = new Random();
		    for(var i = 0; i < 10000; i++)
			{
			    var randomLowest = RandomUtils.GetRandomLowest(vals, x => x, random, 1);
				hits[randomLowest]++;
			}

            Assert.AreEqual(2, hits[1] / hits[2], 0.01);
        }
    }
}
