using System;
using System.Collections.Generic;
using System.Drawing;

namespace MosaicImage
{
    internal class ImageBlocks
    {
        public List<PixelBlock> Blocks { get; set; }

        public int NumBlocksX { get; private set; }
        public int NumBlocksY { get; private set; }

        public int BlockSize { get; set; }

        public ImageBlocks(string sourceImage, int blockSize)
        {
            var sourceBitmap = new Bitmap(sourceImage);
            Console.WriteLine("Reading blocks...");
            NumBlocksX = (int)Math.Floor((double)sourceBitmap.Width / blockSize);
            NumBlocksY = (int)Math.Floor((double)sourceBitmap.Height / blockSize);
            BlockSize = blockSize;
            Blocks = new List<PixelBlock>();
            for (int x = 0; x < NumBlocksX; x++)
            {
                Console.Write("\r" + (x * 100 / NumBlocksX) + "%    ");
                for (int y = 0; y < NumBlocksY; y++)
                {
                    Blocks.Add(new PixelBlock(sourceBitmap, x, y, blockSize));
                }
            }
        }
    }
}