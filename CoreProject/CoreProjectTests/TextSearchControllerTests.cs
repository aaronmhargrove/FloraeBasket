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
    public class TextSearchControllerTests
    {
        // Ensure a garbage search string doesn't return results
        [TestMethod]
        public void TestGarbage()
        {
            // Arrange
            List<FlowerImage> pictureList = new List<FlowerImage>();
            List<Note> daisyNoteList = new List<Note>();
            List<Flower> flowerList = new List<Flower>();
            List<Flower> results = new List<Flower>();
            Note roseNote = new Note("rose rose rose");

            for (int i = 0; i < 3; i++)
            {
                List<Note> roseNoteList = new List<Note>();
                for (int j = 0; j <= i; j++) { roseNoteList.Add(new Note("rose rose rose")); }
                flowerList.Add(new Flower("rose " + (i + 1), "rose " + (i + 1), "rose " + (i + 1), roseNoteList, pictureList));
                roseNoteList.Add(roseNote);
            }

            // Act
            results = new TextSearchController().Search("lkajsdfhlkehuflidsubflsadiufbl", flowerList);
            results = new TextSearchController().OrderFlowers(results);

            // Assert
            Assert.IsTrue(results.Count == 0);
        }

        // Test ordering of flowers by match strength
        [TestMethod]
        public void OrderFlowersTest()
        {
            // Arrange
            List<FlowerImage> pictureList = new List<FlowerImage>();
            List<Flower> flowerList = new List<Flower>();
            List<Flower> sortedList = new List<Flower>();
            Note note = new Note("rose rose rose");

            for(int i = 0; i < 12; i++)
            {
                List<Note> noteList = new List<Note>();
                for (int j = 0; j <= i; j++) { noteList.Add(new Note("rose rose rose")); }
                flowerList.Add(new Flower("rose " + (i + 1), "rose" + (i + 1), "rose" + (i + 1), noteList, pictureList));
                noteList.Add(note);
            }

            // Act
            flowerList = new TextSearchController().Search("rose", flowerList);
            sortedList = new TextSearchController().OrderFlowers(flowerList);

            // Assert
            Assert.IsTrue(sortedList.Count == 10);
            Assert.IsTrue(sortedList[0].GetEnglishName() == "rose 12");
            Assert.IsTrue(sortedList[9].GetEnglishName() == "rose 3");
        }

        // Test match strength rating system
        [TestMethod]
        public void SearchMatchStrengthTest()
        {
            // Arrange
            List<FlowerImage> pictureList = new List<FlowerImage>();
            List<Note> daisyNoteList = new List<Note>();
            List<Flower> flowerList = new List<Flower>();
            List<Flower> sortedList = new List<Flower>();
            Note roseNote = new Note("rose rose rose");
            Flower rose1, rose2, rose3;

            for (int i = 0; i < 3; i++)
            {
                List<Note> roseNoteList = new List<Note>();
                for(int j = 0; j <= i; j++) { roseNoteList.Add(new Note("rose rose rose")); }
                flowerList.Add(new Flower("rose " + (i + 1), "rose " + (i + 1), "rose " + (i + 1), roseNoteList, pictureList));
                roseNoteList.Add(roseNote);
            }

            // Act
            flowerList = new TextSearchController().Search("rose", flowerList);
            rose1 = flowerList[0];
            rose2 = flowerList[1];
            rose3 = flowerList[2];

            // Assert
            Assert.IsTrue(rose1.GetMatches() == 21);
            Assert.IsTrue(rose2.GetMatches() == 24);
            Assert.IsTrue(rose3.GetMatches() == 27);
        }

        // SearchByText() is not tested because it just pulls info from the DBMgr and then uses the tested methods.
        // SearchByColor(color:String) not tested because all business logic is handled by SearchByHistogramController()
        //          -> See SearchHistogramControllerTests
    }
}
