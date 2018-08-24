using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace CoreProject.Tests
{
    [TestClass()]
    public class ViewHistogramControllerTests
    {
        [TestMethod()]
        public void AddDataPointsToChartTest()
        {
            TestPositiveBinValues();
            TestEmptyBins();
            TestNegativeBinValues();
        }

        public static void TestPositiveBinValues()
        {
            // Arrange
            int[] rBin = new int[256];
            int[] gBin = new int[256];
            int[] bBin = new int[256];

            // Set constants for each array value that will be used
            int redPixels = 47;
            int greenPixels = 73;
            int bluePixels = 2000;

            // Fill in dummy rgb values
            for (int i = 0; i < 256; i++)
            {
                rBin[i] = redPixels;
                gBin[i] = greenPixels;
                bBin[i] = bluePixels;
            }

            // Create the histogram 
            Histogram hist = new Histogram(rBin, gBin, bBin);
            int binSize = hist.GetBinSize();

            // Expected values per bin is the sum of each array index n times, where n = binSize
            // The bin values will always be the same for each color since the dummy data for each color is the same
            int expectedRedValues = redPixels * binSize;
            int expectedGreenValues = greenPixels * binSize;
            int expectedBlueValues = bluePixels * binSize;

            Dictionary<string, int> expectedValues = new Dictionary<string, int>();
            expectedValues.Add("Red Values", expectedRedValues);
            expectedValues.Add("Green Values", expectedGreenValues);
            expectedValues.Add("Blue Values", expectedBlueValues);

            // Create a new chart with a matching series name to compare with
            Chart chart = new Chart();
            chart.Series.Add(new Series("Red Pixel Count"));
            chart.Series.Add(new Series("Green Pixel Count"));
            chart.Series.Add(new Series("Blue Pixel Count"));

            // Act
            ViewHistogramController.AddDataPointsToChart(hist, ref chart);

            // Assert

            /*
             * Positive rgb value equivalence partition
             */
            TestAddPointsToChart(chart, expectedValues, binSize);
        }

        public static void TestEmptyBins()
        {
            // Arrange
            int[] rBin = new int[256];
            int[] gBin = new int[256];
            int[] bBin = new int[256];

            // Set constants for each array value that will be used
            int redPixels = 0;
            int greenPixels = 0;
            int bluePixels = 0;

            // Create the histogram 
            Histogram hist = new Histogram(rBin, gBin, bBin);
            int binSize = hist.GetBinSize();

            // Expected values per bin is the sum of each array index n times, where n = binSize
            // The bin values will always be the same for each color since the dummy data for each color is the same
            int expectedRedValues = redPixels * binSize;
            int expectedGreenValues = greenPixels * binSize;
            int expectedBlueValues = bluePixels * binSize;

            Dictionary<string, int> expectedValues = new Dictionary<string, int>();
            expectedValues.Add("Red Values", expectedRedValues);
            expectedValues.Add("Green Values", expectedGreenValues);
            expectedValues.Add("Blue Values", expectedBlueValues);

            // Create a new chart with a matching series name to compare with
            Chart chart = new Chart();
            chart.Series.Add(new Series("Red Pixel Count"));
            chart.Series.Add(new Series("Green Pixel Count"));
            chart.Series.Add(new Series("Blue Pixel Count"));

            // Act
            ViewHistogramController.AddDataPointsToChart(hist, ref chart);

            // Assert
            TestAddPointsToChart(chart, expectedValues, binSize);
        }
        public static void TestNegativeBinValues()
        {
            // Arrange
            int[] rBin = new int[256];
            int[] gBin = new int[256];
            int[] bBin = new int[256];

            // Set constants for each array value that will be used
            int redPixels = -3;
            int greenPixels = -72;
            int bluePixels = -956;

            // Fill in dummy rgb values
            for (int i = 0; i < 256; i++)
            {
                rBin[i] = redPixels;
                gBin[i] = greenPixels;
                bBin[i] = bluePixels;
            }

            // Create the histogram 
            Histogram hist = new Histogram(rBin, gBin, bBin);
            int binSize = hist.GetBinSize();

            // Expected values per bin is the sum of each array index n times, where n = binSize
            // The bin values will always be the same for each color since the dummy data for each color is the same
            int expectedRedValues = redPixels * binSize;
            int expectedGreenValues = greenPixels * binSize;
            int expectedBlueValues = bluePixels * binSize;

            Dictionary<string, int> expectedValues = new Dictionary<string, int>();
            expectedValues.Add("Red Values", expectedRedValues);
            expectedValues.Add("Green Values", expectedGreenValues);
            expectedValues.Add("Blue Values", expectedBlueValues);

            // Create a new chart with a matching series name to compare with
            Chart chart = new Chart();
            chart.Series.Add(new Series("Red Pixel Count"));
            chart.Series.Add(new Series("Green Pixel Count"));
            chart.Series.Add(new Series("Blue Pixel Count"));

            // Act
            ViewHistogramController.AddDataPointsToChart(hist, ref chart);

            // Assert
            TestAddPointsToChart(chart, expectedValues, binSize);
        }

        public static void TestAddPointsToChart(Chart chart, Dictionary<String, int> expectedValues, int binSize)
        {
            int currentBinMax;
            int currentRGBValue;

            // Iterate through each color series in the cart
            foreach (Series series in chart.Series)
            {
                currentBinMax = binSize;
                currentRGBValue = 0;

                // Iterate through the data points in the color
                foreach (DataPoint point in series.Points)
                {
                    String axisLabel = currentRGBValue + " to " + currentBinMax;

                    Assert.AreEqual(point.AxisLabel, axisLabel);

                    if (series.Name == "Red Pixel Count")
                    {
                        // Check that the red values are correct
                        Assert.AreEqual(point.YValues[0], expectedValues["Red Values"]);
                    }
                    else if (series.Name == "Green Pixel Count")
                    {
                        // Check that the green values are correct
                        Assert.AreEqual(point.YValues[0], expectedValues["Green Values"]);
                    }
                    else if (series.Name == "Blue Pixel Count")
                    {
                        // Check that the blue values are correct
                        Assert.AreEqual(point.YValues[0], expectedValues["Blue Values"]);
                    }

                    // Moves to the next bin for labeling
                    currentRGBValue = currentBinMax;
                    currentBinMax += binSize;
                }
            }
        }
    }
}