using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicImage
{
    class Program
    {
        static void Main()
        {
            const int sourceBlockSize = 10;
            int targetBlockSize = 100;
            const string availableImagesDir = @"c:\temp\mosaic\siri";

            var availableFiles = Generator.ReadAvailableImages(availableImagesDir, targetBlockSize).ToArray();

            string sourceDir = @"c:\temp\mosaic\src";
            foreach (var sourceImageFile in Directory.GetFiles(sourceDir, "*.jpg"))
            {
                string sourceImage = Path.Combine(sourceDir, sourceImageFile);
                var targetImagePath = Path.ChangeExtension(sourceImage, sourceBlockSize + "-" + targetBlockSize + "_target.jpg");
                if (!sourceImageFile.Contains("_target") && !File.Exists(targetImagePath))
                    Generator.GenerateMosaicImage(sourceImage, sourceBlockSize, targetBlockSize, targetImagePath, availableFiles);
            }
        }
    }
}
