//
// FILE: Histogram.cs
// INFO: Data class for histogram object (used for image comparison)
//

using System.Drawing;

namespace CoreProject
{
    public class Histogram
    {
        private int id;
        private int[] rBin = new int[256];
        private int[] gBin = new int[256];
        private int[] bBin = new int[256];
        private int flowerId;
        private int flowerImageId;
        private int binSize = 32;

        // Constructor based on an image
        public Histogram(Image image)
        {
            Bitmap imageBitMap = (Bitmap) image;

            // Loop through each pixel in the 2D image
            for (int x = 0; x < imageBitMap.Width; x++)
            {
                for (int y = 0; y < imageBitMap.Height; y++)
                {
                    // Get the color of the current pixel
                    Color c = imageBitMap.GetPixel(x, y);

                    // Add the occurrences for every specified color level in our bins
                    rBin[c.R] = rBin[c.R] + 1;
                    gBin[c.G] = gBin[c.G] + 1;
                    bBin[c.B] = bBin[c.B] + 1;
                }
            }
        }

        // Alt ctor based on RGB
        public Histogram(int r, int g, int b)
        {
            this.rBin[r] = 1;
            this.gBin[g] = 1;
            this.bBin[b] = 1;
        }

        // Alt ctor for RGB array
        public Histogram(int[] r, int[] g, int[] b)
        {
            this.rBin = r;
            this.gBin = g;
            this.bBin = b;
        }

        // Alt ctor for DB retrieval (keeping id relationships)
        public Histogram(int histId, int[] r, int[] g, int[] b, int flowerId, int flowerImageId)
        {
            this.id = histId;
            this.rBin = r;
            this.gBin = g;
            this.bBin = b;
            this.flowerId = flowerId;
            this.flowerImageId = flowerImageId;
        }

        // Getters
        public int[] GetRBin() { return this.rBin; }
        public int[] GetGBin() { return this.gBin; }
        public int[] GetBBin() { return this.bBin; }
        public int GetBinSize() { return this.binSize; }
        public int GetFlowerId() { return this.flowerId; }

        // Setters
        public void SetId(int id) { this.flowerId = id; }
    }
}
