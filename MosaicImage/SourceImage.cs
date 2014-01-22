using System;
using System.Collections.Generic;
using System.Drawing;

namespace MosaicImage
{
    public class SourceImage
    {
        public List<SourcePixelBlock> Blocks { get; set; }

        public int NumBlocksX { get; private set; }
        public int NumBlocksY { get; private set; }

        public int BlockSize { get; set; }

        public SourceImage(int sourceBlockSize, Bitmap sourceBitmap)
        {
            Console.WriteLine("Reading blocks...");
            NumBlocksX = (int)Math.Floor((double)sourceBitmap.Width / sourceBlockSize);
            NumBlocksY = (int)Math.Floor((double)sourceBitmap.Height / sourceBlockSize);
            BlockSize = sourceBlockSize;
            Blocks = new List<SourcePixelBlock>();
            for (int x = 0; x < NumBlocksX; x++)
            {
                Console.Write("\r" + (x * 100 / NumBlocksX) + "%    ");
                for (int y = 0; y < NumBlocksY; y++)
                {
                    Blocks.Add(new SourcePixelBlock(sourceBitmap, x, y, sourceBlockSize));
                }
            }
        }
    }
}