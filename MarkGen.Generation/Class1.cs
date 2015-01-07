using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ExifLib;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace MarkGen.Generation {
    public class GalleryGenerator {
        public void Generate(string galleryName, string sourceFilePath) {
            var filesInDirectory = Directory.EnumerateFiles(sourceFilePath).Where(x=> !x.Contains("Thumbs.db"));

            var targetPath = @"C:\MarkGen\output\g\" + galleryName.Replace(" ", "_");
            var i = new ImageProcessor();

            //1. Create Gallery Folder
            //Make a copy of the 'web' folder in the Juicebox download folder. This will be your gallery folder.
            CopyGalleryTemplate(targetPath);
            var config = new List<string>();
            config.Add(string.Format("<juiceboxgallery galleryTitle=\"{0}\">", galleryName));

            foreach (var sourceFile in filesInDirectory)
            {
                var fileInfo = new FileInfo(sourceFile);
                var targetFileForImages = Path.Combine(targetPath, "images", fileInfo.Name);
                var targetFileForThumbs = Path.Combine(targetPath, "thumbs", fileInfo.Name);
                i.CreateThumbnailAndResizedImage(sourceFile, targetFileForThumbs, targetFileForImages, 25);

                config.Add(string.Format("<image imageURL=\"images/{0}\" thumbURL=\"thumbs/{0}\" linkURL=\"images/{0}\" linkTarget=\"_blank\"/>"  ,fileInfo.Name));
            }
            config.Add("</juiceboxgallery>");

            File.WriteAllLines(targetPath + @"\config.xml", config);
        }

        private void CopyGalleryTemplate(string targetPath) {
            var process = new Process();
            process.StartInfo.FileName = "xcopy";
            var source = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            process.StartInfo.Arguments = string.Format(@"{0} {1} /e /h /c /I", source + @"\galleryTemplate", targetPath);
            process.Start();  
        }
    }

    public class ImageProcessor {

        public void CreateThumbnailAndResizedImage(string sourcePath, string thumbnailPath, string targetPath, int newSizeInPercent) {
            var sourceStream = new StreamReader(sourcePath).BaseStream;
            CreateThumbnail(sourceStream, thumbnailPath);
            ResizeImage(sourceStream, targetPath, newSizeInPercent);
        }

        public void CreateThumbnail(Stream sourceStream, string thumbnailPath) {
            const int thumbSize = 85;
            var imageFactory = new ImageFactory();
            imageFactory = imageFactory.Load(sourceStream);
            var resizeLayer = new ResizeLayer(new Size(thumbSize, thumbSize),ResizeMode.Crop);
            imageFactory.Resize(resizeLayer).AutoRotate().Save(thumbnailPath);
            imageFactory.Dispose();
        }

        public void ResizeImage(Stream sourceStream, string targetPath, int newSizeInPercent) {
            var imageFactory = new ImageFactory();
            imageFactory = imageFactory.Load(sourceStream);
            var size = CalcNewSize(imageFactory.Image.Size, newSizeInPercent);
            imageFactory.Resize(size).AutoRotate().Save(targetPath);
            imageFactory.Dispose();

        }

        private Size CalcNewSize(Size oldSize, int newSizeInPercent) {
            var newHeight = Convert.ToInt32((oldSize.Height / 100) * newSizeInPercent);
            var newWidth = Convert.ToInt32((oldSize.Width / 100) * newSizeInPercent);
            return new Size(newWidth, newHeight);
        }        
    }
}
