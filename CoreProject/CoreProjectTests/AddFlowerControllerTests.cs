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
    public class AddFlowerControllerTests
    {
        // Test the method with valid inputs, result should be true
        [TestMethod()]
        public void addEntryTestTrue()
        {
            List<FlowerImage> pictures = new List<FlowerImage>();
            List<String> newNotes = new List<String>();
            newNotes.Add("test note 1");
            newNotes.Add("test note 2");
            bool result = AddFlowerController.AddEntry("Rose", "Rosa", "Rosaceae", newNotes, pictures);
            Assert.IsTrue(result);
        }
        
        // Test the method with invalid inputs, result should be false
        [TestMethod()]
        public void addEntryTestFalse()
        {
            List<FlowerImage> pictures = new List<FlowerImage>();
            List<String> newNotes = new List<String>();
            newNotes.Add("test note 1");
            newNotes.Add("test note 2");
            bool result = AddFlowerController.AddEntry("", "Rosa", "Rosaceae", newNotes, pictures);
            Assert.IsFalse(result);

            result = AddFlowerController.AddEntry("Rose", "", "Rosaceae", newNotes, pictures);
            Assert.IsFalse(result);

            result = AddFlowerController.AddEntry("Rose", "Rosa", "", newNotes, pictures);
            Assert.IsFalse(result);
        }
    }
}