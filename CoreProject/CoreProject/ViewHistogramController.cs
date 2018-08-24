//
// FILE: ViewHistogramController.cs
// INFO: Handles chart creation
//

using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace CoreProject
{
    public class ViewHistogramController
    {
        // Adds points from histogram into GUI chart
        // Takes histogram and reference to chart object
        public static void AddDataPointsToChart(Histogram selectedHistogram, ref Chart colorHistogram)
        {
            // Clear chart before adding new data
            foreach (Series series in colorHistogram.Series)
            {
                series.Points.Clear();
            }

            // Instantiate bin values
            int[] rBin = selectedHistogram.GetRBin();
            int[] gBin = selectedHistogram.GetGBin();
            int[] bBin = selectedHistogram.GetBBin();

            
            int binSize = selectedHistogram.GetBinSize();

            // Used to create the label for which rgb values the bins contain
            int currentBinMax = binSize;
            int currentRGBValue = 0;

            // Keeps running total of each color in each bin
            int redTotal = 0;
            int greenTotal = 0;
            int blueTotal = 0;

            String axisLabel;

            for (int i = 0; currentRGBValue < 256; i++)
            {
                // Reset the running total for each bin being iterated through
                redTotal = 0;
                greenTotal = 0;
                blueTotal = 0;

                // Iterates through each bin 
                for (int j = currentRGBValue; j < currentBinMax; j++)
                {
                    redTotal += rBin[j];
                    greenTotal += gBin[j];
                    blueTotal += bBin[j];
                }

                axisLabel = currentRGBValue + " to " + currentBinMax;

                // Add each individual point to chart
                colorHistogram.Series["Red Pixel Count"].Points.AddXY(axisLabel, redTotal);
                colorHistogram.Series["Green Pixel Count"].Points.AddXY(axisLabel, greenTotal);
                colorHistogram.Series["Blue Pixel Count"].Points.AddXY(axisLabel, blueTotal);

                // Move to the next bin
                currentRGBValue = currentBinMax;
                currentBinMax += binSize;
            }
            
        }

    }
}
