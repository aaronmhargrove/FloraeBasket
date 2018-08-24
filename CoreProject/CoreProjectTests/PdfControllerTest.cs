using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CoreProject.Tests
{
    [TestClass()]
    public class PdfControllerTest
    {

        [TestMethod()]
        public void TestHtmlBuild()
        {
            // Arrange
            String htmlStringExpeced = "<body style='background-color: #FEEDED'><div><h1 style = 'font-size: 230%; text-decoration: underline;'>" +
                "English</h1><p style='margin-left: 50px;'>Latin Name - Latin</p><p style='margin-left: 50px;'>Botanical Family - Botanical</p><br/>" +
                "<h2 style='margin-left: 50px; text-decoration: underline; font-size: 140%'>Images</h2><div style='margin-left: 70px; height: 400px; " +
                "width: 400px;'><img src = 'C:\\Users\\Owner\\Desktop\\roses.jpg' style = 'height: 100%; width: 100%; object-fit: contain;'></div><h2 style=" +
                "'margin-left: 90px; text-decoration: underline; font-size: 130%'>Image Note</h2><p style='margin-left: 100px;text-indent: 20px;'>" +
                "NoteTest</p><br/><h2 style='margin-left: 50px; text-decoration: underline; font-size: 140%'>Flower Notes</h2><p style='margin-left:" +
                " 70px;text-indent: 20px;'>Testing</p><br/></div></body>";
            Note note = new Note("Testing");
            List<Note> noteList = new List<Note>();
            noteList.Add(note);

            String path = @"C:\Users\Owner\Desktop\roses.jpg";
            FlowerImage img = new FlowerImage(path, "NoteTest");
            List<FlowerImage> imgList = new List<FlowerImage>();
            imgList.Add(img);

            Flower flower = new Flower("English", "Latin", "Botanical", noteList, imgList);
            List<Flower> flowerList = new List<Flower>();
            flowerList.Add(flower);
            PdfController ctrl = new PdfController();

            // Act
            String htmlStringActual = ctrl.GetHtml(flowerList);

            // Assert
            Assert.AreEqual(htmlStringExpeced, htmlStringActual);
        }

        // PDF creation is handled by IronPDF and saving is handled by the OS, so those two methods are not tested.
    }
}
