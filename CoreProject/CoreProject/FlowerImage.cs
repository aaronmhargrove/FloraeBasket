//
// FILE: FlowerImage.cs
// INFO: Class for flower image object
//

using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace CoreProject
{
    public class FlowerImage
    {
        // Class data members
        private String imageLocation;
        private Note note;
        private Image image;
        private Histogram histogram;
        private int id;
        private int histogramId;
        private int flowerId;

        // Ctor (filepath)
        public FlowerImage(String imageLocation)
        {
            Initialize(imageLocation);
        }

        // Ctor (filepath, info)
        public FlowerImage(String imageLocation, String info)
        {
            Initialize(imageLocation);
            this.note = new Note(info);
        }

        // Ctor for constructing FlowerImage from database data
        public FlowerImage(int id, String imageLocation, Note note, int flowerId)
        {
            Initialize(imageLocation);
            this.id = id;
            this.note = note;
            this.flowerId = flowerId;
        }

        // Base ctor functionality
        public void Initialize(String imageLocation)
        {
            this.imageLocation = imageLocation;

            // Checks if the imageLocation is an online url or from the filesystem
            if (Uri.IsWellFormedUriString(imageLocation, UriKind.Absolute))
            {
                // Converts the image from url to bytes array
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(imageLocation);
                MemoryStream ms = new MemoryStream(bytes);

                // Set the image on the class
                SetImage(Image.FromStream(ms));
            }
            else
            {
                SetImage(Image.FromFile(imageLocation));
            }

            SetHistogram(image);
        }

        // Getters
        public String GetImageLocation() { return this.imageLocation; }
        public Note GetNote() { return this.note; }
        public Image GetImage() { return this.image; }
        public Histogram GetHistogram() { return this.histogram; }
        public int GetFlowerId() { return this.flowerId; }
        public int GetId() { return this.id; }

        //Setters
        public void SetImageLocation(String imageLocation) { this.imageLocation = imageLocation; }
        public void SetImage(Image image) { this.image = image; }
        public void SetHistogram(Image image) { this.histogram = new Histogram(image);  }
        public void SetId(int id) { this.id = id; }
        public void SetHistogramId (int histId) { this.histogramId = histId; }
        public void SetFlowerId (int flowerId) { this.flowerId = flowerId; }
        public void SetNote(Note note) { this.note = note; }
    }
}
