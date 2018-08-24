//
// FILE: PhotoController.cs
// INFO: Controller for saving an image with a flower (local directory)
//

using System;
using System.IO;
using System.Windows.Forms;

namespace CoreProject
{
    public static class PhotoController
    {
        // Saves image to folder inside program.
        // Takes FlowerImage as argument
        public static String SaveImage(FlowerImage flowerImage)
        {
            // Describes the path to an images folder in the project's bin folder
            string appPath = Path.GetDirectoryName(Application.StartupPath) + @"\images\";
            string imageName = Path.GetFileName(flowerImage.GetImageLocation());
            
            System.IO.Directory.CreateDirectory(appPath);

            // Save the image to the images folder (internal local directory)
            flowerImage.GetImage().Save(appPath + imageName);

            return appPath + imageName;
        }
    }
}
