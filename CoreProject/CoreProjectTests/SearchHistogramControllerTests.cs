using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreProject.Tests
{
    [TestClass()]
    public class SearchHistogramControllerTests
    {
        [TestMethod()]
        public void ValidateRgbTest()
        {
            TestCommonCase();
            TestNegativeValues();
            TestLargeValues();
        }

        private void TestLargeValues()
        {
            // Arrange 
            int r = 10;
            int g = 32;
            int b = 257;

            // Act
            bool actual = SearchHistogramController.ValidateRgb(r, g, b);

            // Assert
            Assert.IsFalse(actual);
        }

        private void TestNegativeValues()
        {
            // Arrange 
            int r = -5;
            int g = 3;
            int b = -2;

            // Act
            bool actual = SearchHistogramController.ValidateRgb(r, g, b);

            // Assert
            Assert.IsFalse(actual);
        }

        private void TestCommonCase()
        {
            // Arrange 
            int r = 10;
            int g = 32;
            int b = 65;

            // Act
            bool actual = SearchHistogramController.ValidateRgb(r, g, b);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void TestCompareHistograms()
        {
            // Arrange
            Histogram hist1 = new Histogram(30, 30, 30);
            Histogram hist2 = new Histogram(20, 20, 20);

            double expected = .00552;

            // Act
            double actual = Math.Round(SearchHistogramController.CompareHistograms(hist1, hist2), 5);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TestGetTopThreeMatches()
        {
            // Arrange
            Flower flower1 = new Flower("test1", "test1", "test1", new List<Note>(), new List<FlowerImage>());
            Flower flower2 = new Flower("test2", "test2", "test2", new List<Note>(), new List<FlowerImage>());
            Flower flower3 = new Flower("test3", "test3", "test3", new List<Note>(), new List<FlowerImage>());

            Dictionary<double, Flower> flowers = new Dictionary<double, Flower>();
            flowers.Add(.123, flower1);
            flowers.Add(.056, flower2);
            flowers.Add(.5, flower3);

            List<Flower> expected = new List<Flower>();
            expected.Add(flower2);
            expected.Add(flower1);
            expected.Add(flower3);

            // Act
            List<Flower> actual = SearchHistogramController.GetTopThreeMatches(flowers);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}