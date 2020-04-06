using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoboHash.Services
{
    public class RobotService
    {
        public List<long> CreateHashes(string hexDigest, int count)
        {
            var hashedArray = new List<long>();
            for (int i = 0; i < count; i++)
            {
                var blockSize = (int)(hexDigest.Length / count);
                var currentStart = (1 + i) * blockSize - blockSize;
                var hex = hexDigest.Substring(currentStart, blockSize);
                hashedArray.Add(Convert.ToInt64(hex, 16));
            }
            return hashedArray;
        }

        public string GetColor(long hashInput)
        {
            var colors = new List<string>
            {
                "blue", "brown", "green", "grey", "orange", "pink", "purple", "red", "white", "yellow"
            };
            var color = colors[(int)(hashInput % colors.Count)];
            return color;
        }

        public Image Assemble(List<long> hashArray, string color, int width, int height)
        {
            string assetPath = Directory.GetCurrentDirectory();
            // first get a list of parts of our robot
            var partsPath = Path.Combine(assetPath, "sets", "set1", color);
            List<string> roboParts = GetListOfFiles(hashArray, partsPath);
            roboParts = roboParts.OrderBy(x => x.Substring(x.IndexOf('#'))).ToList();

            var finalImage = Image.Load(roboParts[0]);
            foreach (var roboPart in roboParts)
            {
                var image = Image.Load(roboPart);
                finalImage.Mutate(ctx => ctx.DrawImage(image, 1));
            }

            finalImage.Mutate(x => x.Resize(width, height));

            return finalImage;
        }

        public List<string> GetListOfFiles(List<long> hashArray, string path)
        {
            var chosenFiles = new List<string>();
            var directories = Directory.GetDirectories(path).OrderBy(x => x);

            int iter = 4;
            foreach (var directory in directories)
            {
                var files = Directory.GetFiles(directory).OrderBy(x => x).ToList();
                var elementInList = (int)(hashArray[iter] % files.Count);
                chosenFiles.Add(files[elementInList]);
                iter += 1;
            }
            return chosenFiles;
        }
    }
}
