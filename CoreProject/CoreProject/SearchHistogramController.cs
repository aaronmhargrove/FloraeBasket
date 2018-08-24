//
// FILE: SearchHistogramController.cs
// INFO: Controller for searching by histogram (image matching and RGB)
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreProject
{
    public class SearchHistogramController
    {
        private static double matchThreshold = 3.7000;  // Threshold for matching similar images by color content 
                                                        // 0 being exact match and 10 being no match at all
                                                        // Measured as the "distance" from an exact match 
                                                        // Value may be adjusted to dial in strength of match

        // Assures all entered values are valid (0-255)
        // Returns true or false.
        public static bool ValidateRgb(int r, int g, int b)
        {
            if (r > 255 || r < 0 || g > 255 || g < 0 || b > 255 || b < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Searches for matches based on RGB values input by the user.
        // Takes (int) red value, (int) green value, (int) blue value. Returns list of flowers.
        public static List<Flower> SearchByHistogram(int r, int g, int b)
        {
            Histogram hist = new Histogram(r, g, b);
            
            return SearchByHistogram(hist);
        }

        // Searches for match based on histogram created from user uploaded image.
        // Takes Histogram object. Returns list of Flower objects (matches.)
        public static List<Flower> SearchByHistogram(Histogram searchHist)
        {
            List<Flower> flowers;
            List<Histogram> histograms = DBMgr.GetInstance().GetHistograms();

            // Double == chi threshold value. Flower is associated object
            Dictionary<double, Flower> flowerMatchValues = new Dictionary<double, Flower>();
            double chiValue;

            foreach (Histogram hist in histograms)
            {
                chiValue = CompareHistograms(searchHist, hist);

                // If the chi value is below threshold. Get associated flower object and add to match list
                if (chiValue <= matchThreshold && !flowerMatchValues.ContainsKey(chiValue))
                {
                    Flower flower = DBMgr.GetInstance().GetFlowerByHistogram(hist);
                    flowerMatchValues.Add(chiValue, flower);
                }
            }

            // Get top 3 matches from list
            flowers = GetTopThreeMatches(flowerMatchValues);

            return flowers;
        }

        // Takes 2 histograms and returns the average chi distance value.
        public static double CompareHistograms(Histogram hist1, Histogram hist2)
        {
            double redChi = 0, blueChi = 0, greenChi = 0;
            double squared = 0;
            double squaredOverDistance = 0;
            double matchingValue = 0;

            // Sum the red bins
            int[] rBin1 = hist1.GetRBin();
            int[] rBin2 = hist2.GetRBin();
            for (int i = 0; i <= 255; i++)
            {
                int x = rBin1[i];
                int y = rBin2[i];

                squared = Math.Pow((x - y), 2);

                if((x + y) != 0)
                {
                    squaredOverDistance = squared / (x + y);
                    redChi += squaredOverDistance;
                }
            }

            redChi = Math.Sqrt(Math.Abs(redChi)) / 256.0;

            // Sum the green bins
            int[] gBin1 = hist1.GetGBin();
            int[] gBin2 = hist2.GetGBin();
            for (int i = 0; i <= 255; i++)
            {
                int x = gBin1[i];
                int y = gBin2[i];
                squared = Math.Pow((x - y), 2);

                if ((x + y) != 0)
                {
                    squaredOverDistance = squared / (x + y);
                    greenChi += squaredOverDistance;
                }
            }

            greenChi = Math.Sqrt(Math.Abs(greenChi)) / 256.0;

            // Sum the blue bins
            int[] bBin1 = hist1.GetBBin();
            int[] bBin2 = hist2.GetBBin();
            for (int i = 0; i <= 255; i++)
            {
                int x = bBin1[i];
                int y = bBin2[i];
                squared = Math.Pow((x - y), 2);

                if ((x + y) != 0)
                {
                    squaredOverDistance = squared / (x + y);
                    blueChi += squaredOverDistance;
                }
            }

            blueChi = Math.Sqrt(Math.Abs(blueChi)) / 256.0;

            // Average the chi values
            matchingValue = (redChi + greenChi + blueChi) / 3;

            return matchingValue;
        }
        
        // Takes dictionary. Returns list of top Flower object matches.
        public static List<Flower> GetTopThreeMatches(Dictionary<double, Flower> flowerMatches)
        {
            List<Flower> flowers = new List<Flower>();
            List<double> chiValues = new List<double>(flowerMatches.Keys);

            // Sort the list by chi values (acending)
            chiValues.Sort();
            chiValues = chiValues.Take(3).ToList();

            // Add associated flowers to the flowers list
            foreach (double chi in chiValues)
            {
                flowers.Add(flowerMatches[chi]);
            }

            return flowers;
        }
    }
}
