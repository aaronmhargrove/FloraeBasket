//
// FILE: EditFlowerController.cs
// INFO: Class for controlling business logic behind editing an existing flower
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject
{
    public class EditFlowerController
    {
        // Updates flower info and tells DBMgr to update flower with new data
        // Takes English name, Latin name, botanical family, list of Note objects, list of FlowerImage objects, and the flower object being edited
        // Returns success or failure
        public static bool EditEntry(String engName, String latName, String family, List<String> notes, List<FlowerImage> pictures, Flower editFlower)
        {
            List<Note> noteList = new List<Note>();
            // Make updates only to fields that were changed
            if (!engName.Equals(editFlower.GetEnglishName()) && !engName.Equals(""))
            {
                editFlower.SetEnglishName(engName);
            }
            if (!latName.Equals(editFlower.GetLatinName()) && !latName.Equals(""))
            {
                editFlower.SetLatinName(latName);
            }
            if (!family.Equals(editFlower.GetBotanicalFamily()) && !family.Equals(""))
            {
                editFlower.SetBotanicalFamily(family);
            }
            for (int i = 0; i < notes.Count(); i++)
            {
                    noteList.Add(new Note(notes[i]));
                    editFlower.SetNotes(noteList);
            }
            if (pictures != null)
            {
                editFlower.SetImages(pictures);
            }
            // Tell DBMgr to add the flower to the database
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.EditFlower(editFlower);
            // Determine success or failure and return
            if (success) { return true; }
            else { return false; }
        }

        // Tells DBMgr to delete a specific flower
        // Takes a flower as an argument and returns success or failure
        public static bool DeleteEntry(Flower deleteFlower)
        {
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.DeleteFlower(deleteFlower);
            if(success) { return true; }
            else { return false; }
        }
    }
}
