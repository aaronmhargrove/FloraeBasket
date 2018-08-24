//
// FILE: Flower.cs
// INFO: Data class for flower object
//

using System;
using System.Collections.Generic;

namespace CoreProject
{
    public class Flower
    {
        // Class Data Members
        private String englishName;
        private String latinName;
        private String botanicalFamily;
        private List<Note> notes = new List<Note>();
        private List<FlowerImage> pictures = new List<FlowerImage>();
        private int id;
        private int matchVal = 0;

        // Designated ctor
        // Takes English namme, Latin name, botanical family, flower note strings, image location (filepath), picture note strings
        public Flower(String englishName, String latinName, String botanicalFamily, List<String> flowerInfo, List<String> imageLocations, List<String> pictureInfo)
        {
            this.englishName = englishName;
            this.latinName = latinName;
            this.botanicalFamily = botanicalFamily;
            for(int i = 0; i < flowerInfo.Count; i++)
            {
                this.notes.Add(new Note(flowerInfo[i]));
            }
            
            for (int i = 0; i < imageLocations.Count; i++)
            {
                String tempImage = imageLocations[i];
                String tempPicInfo = pictureInfo[i];
                pictures.Add(new FlowerImage(tempImage, tempPicInfo));
            }
        }

        // Alternate constructor for creating a flower with a premade list of FlowerImage objects
        // Takes English name, Latin name, botanical family, List of flower note strings, list of FlowerImage objects
        public Flower(String englishName, String latinName, String botanicalFamily, List<String> flowerInfo, List<FlowerImage> pictures)
        {
            List<Note> noteList = new List<Note>();
            this.englishName = englishName;
            this.latinName = latinName;
            this.botanicalFamily = botanicalFamily;
            if (flowerInfo.Equals("")) { noteList.Add(new Note("Flower added to system.")); }
            else {
                for (int i = 0; i < flowerInfo.Count; i++)
                {
                    noteList.Add(new Note(flowerInfo[i]));
                }
            }
            this.pictures = pictures;
            this.notes = noteList;
        }

        // Alt Ctor for DB retrevial
        // Takes English name, Latin name, botanical family, List of flowers' Note objects, list of FlowerImage objects
        public Flower(String englishName, String latinName, String botanicalFamily, List<Note> flowerNotes, List<FlowerImage> images)
        {
            this.englishName = englishName;
            this.latinName = latinName;
            this.botanicalFamily = botanicalFamily;
            this.notes = flowerNotes;
            this.pictures = images;
        }

        // Alt Ctor for DB retrevial using flower ID
        // Takes ID, English name, Latin name, botanical family, List of flowers' Note objects, list of FlowerImage objects
        public Flower(int id, String englishName, String latinName, String botanicalFamily, List<Note> flowerNotes, List<FlowerImage> images)
        {
            this.id = id;
            this.englishName = englishName;
            this.latinName = latinName;
            this.botanicalFamily = botanicalFamily;
            this.notes = flowerNotes;
            this.pictures = images;
        }


        // Getters
        public String GetEnglishName() { return this.englishName; }
        public String GetLatinName() { return this.latinName; }
        public String GetBotanicalFamily() { return this.botanicalFamily; }
        public int GetId() { return this.id; }
        public List<Note> GetNotes() { return this.notes; }
        public Note GetNote(int i ) { return this.notes[i]; }
        public List<FlowerImage> GetImages() { return pictures; }
        public int GetMatches() { return matchVal; }

        // Setters
        public void SetEnglishName(String newEngName) { this.englishName = newEngName; }
        public void SetLatinName(String newLatName) { this.latinName = newLatName; }
        public void SetBotanicalFamily(String newFamily) { this.botanicalFamily = newFamily; }
        public void SetNotes(List<Note> notes) { this.notes = notes; }
        public void SetImages(List<FlowerImage> newImages) { this.pictures = newImages; }
        public void SetId(int id) { this.id = id; }
        // Adds value to flower's match value
        // Used to determine match strength of search
        public void AddMatchVal(int num) { this.matchVal += num; }
    }
}