//
// FILE: AddFlowerController.cs
// INFO: Class for controlling business logic behind adding a flower to the database.
//

using System;
using System.Collections.Generic;

namespace CoreProject
{
    public class AddFlowerController
    {
        // Contains one function to add a flower entry. Returns either success or failure.
        // Takes English name, Latin name, Botanical family, Note objects for flower, list of FlowerImage objects
        public static bool AddEntry(String engName, String latName, String family, List<String> notes, List<FlowerImage> pictures)
        {
            // Make sure there was input in required fields
            if (engName.Equals("") || latName.Equals("") || family.Equals(""))
            {
                return false;
            }

            // Save pictures and change path to local directory
            if (pictures != null)
            {
                foreach (FlowerImage image in pictures)
                {
                    String newPath = PhotoController.SaveImage(image);
                    image.SetImageLocation(newPath);
                }
            }
            
            Flower newFlower = new Flower(engName, latName, family, notes, pictures);

            // Add the flower to the database
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.SaveNewFlower(newFlower);

            // Return success or failure
            if (success) { return true; }
            else { return false; }
        }
    }
}
