using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Tests
{
    [TestClass()]
    public class EditFlowerControllerTests
    {
        [TestMethod()]
        public void editEntryTest()
        {
            // Assess
            List<Note> testNotes = new List<Note>();
            testNotes.Add(new Note("Test note"));
            List<FlowerImage> testImages = new List<FlowerImage>();
            Flower testFlower = new Flower("rose", "rosa", "rosacea", testNotes, testImages);
            List<String> newNotes = new List<String>();
            newNotes.Add("test note 1");
            newNotes.Add("test note 2");
            List<String> newNotes2 = new List<String>();
            newNotes.Add("test note 3");
            newNotes.Add("test note 4");

            // Basis Path 1 - All inputs match existing flower
            // Act
            bool result = EditFlowerController.EditEntry("rose", "rosa", "rosacea", newNotes, testImages, testFlower);
            // Assert
            Assert.IsTrue(result);

            // Basis Path 2 - Flower english names do not match
            // Act
            result = EditFlowerController.EditEntry("daisy", "rosa", "rosacea", newNotes, testImages, testFlower);
            // Assert
            Assert.IsTrue(result);

            // Basis Path 3 - Flower latin names do not match
            // Act
            result = EditFlowerController.EditEntry("rose", "daisy", "rosacea", newNotes, testImages, testFlower);
            //Assert
            Assert.IsTrue(result);

            // Basis Path 4 - Flower botanical families do not match
            // Act
            result = EditFlowerController.EditEntry("rose", "rosa", "daisy", newNotes, testImages, testFlower);
            // Assert
            Assert.IsTrue(result);

            // Basis Path 5 - Flower notes do not match
            // Act
            result = EditFlowerController.EditEntry("rose", "rosa", "rosacea", newNotes2, testImages, testFlower);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void deleteEntryTest()
        {
            // Assess
            List<Note> testNotes = new List<Note>();
            testNotes.Add(new Note("Test note"));
            List<FlowerImage> testImages = new List<FlowerImage>();
            Flower testFlower = new Flower("rose", "rosa", "rosacea", testNotes, testImages);
            // Act
            bool result = EditFlowerController.DeleteEntry(testFlower);
            //Assert
            Assert.IsTrue(result);
        }
    }
}