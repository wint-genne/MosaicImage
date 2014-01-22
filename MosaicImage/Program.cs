using System.Collections;
using System.Drawing.Imaging;
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
            Generator.GenerateMosaicImage(sourceImage, blockSize, blockTargetSize, availableImagesDir);
        }
    }
}
