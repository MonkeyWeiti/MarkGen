using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace MarkGen.Generation {
    public class GalleryGenerator {
        public void Generate(string galleryName, string sourceFilePath) {
            var filesInDirectory = Directory.EnumerateFiles(sourceFilePath);

            var targetPath = "C:\\MarkGen\\output\\g\\" + galleryName.Replace(" ", "_");
            var i = new ImageProcessor();

            //1. Create Gallery Folder
            //Make a copy of the 'web' folder in the Juicebox download folder. This will be your gallery folder.

            foreach (var sourceFile in filesInDirectory)
            {
                var fileInfo = new FileInfo(sourceFile);
                var targetFileForImages = Path.Combine(targetPath, "images", fileInfo.Name);
                var targetFileForThumbs = Path.Combine(targetPath, "thumbs", fileInfo.Name);

                //2. Add Images
                //Copy your images to the gallery folder 'images' folder.
                i.ResizeImage(sourceFile,targetFileForImages,25);

                //3. Create Thumbnails
                //Create thumbnails with an image editing program (e.g. Photoshop). Place thumbnail images in 'thumbs' folder. Thumbnails should be square and at least 85x85 pixels.
                i.CreateThumbnail(sourceFile,targetFileForThumbs);

                //4: Edit config.xml
                //Open config.xml in any text editing software (e.g. Notepad, TextEdit ). Set your gallery options by editing the juiceboxgallery tag attributes at the top of the file. View details on setting config options.
                //Next, add an <image> tag for every image in the gallery:
                //  <image imageURL="images/wide.jpeg" thumbURL="thumbs/wide.jpeg" linkURL="images/wide.jpeg" linkTarget="_blank"/>  
            }

        }
    }

    public class ImageProcessor {
        public void CreateThumbnail(string sourcePath, string thumbnailPath) {
            var image = Image.FromFile(sourcePath);
            const int thumbSize = 85;
            var thumbnail = ResizeImage(image, thumbSize, thumbSize);
            thumbnail.Save(thumbnailPath);
        }
        public void ResizeImage(string sourcePath, string targetPath, int newSizeInPercent) {
            var image = Image.FromFile(sourcePath);
            var newHeight = (image.Height / 100) * newSizeInPercent;
            var newWidth = (image.Width / 100) * newSizeInPercent;
            var resizedImage = ResizeImage(image, newWidth, newHeight);
            resizedImage.Save(targetPath);
        }

        public static Image ResizeImage(Image imgToResize, int width, int height) {
            var resizeImage = (Image)(new Bitmap(imgToResize, new Size(width, height)));
            imgToResize.Dispose();
            return resizeImage;
        }
    }
}
