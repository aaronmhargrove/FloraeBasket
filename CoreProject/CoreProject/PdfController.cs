//
// FILE: PdfController.cs
// INFO: Controller for creating and saving pdf of search results
// LIBRARY DEPENDENT: This class is dependent on the IronPdf library. It is a pdf library free for development testing and personal use (TRIAL LICENSE).
//      May not be deployed to clients without proper license. Personal git only (Such as all pdf libraries. In short don't sell/distribute this product.)
//      It uses html to create a pdf file that can be saved and/or printed.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CoreProject
{
    public class PdfController
    {
        // Ctor
        public PdfController() { }
       
        // Saves PDF at user selected location
        // Takes list of search results
        public void SavePDF(List<Flower> results)
        {
            IronPdf.PdfDocument PDF = CreatePDF(results);

            // Get desired save location and file name from user
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF document (*.pdf)|*.pdf";
            DialogResult result = saveDialog.ShowDialog();
            String fileName = saveDialog.FileName;
            if (result == DialogResult.OK)
            {
                // Save PDF
                Path.GetFullPath(saveDialog.FileName);
                String path = Path.GetFullPath(saveDialog.FileName);
                PDF.SaveAs(path);

                // Display pdf to user
                System.Diagnostics.Process.Start(path);
            }
        }

        // Creates pdf document
        // Takes list of Flower objects as argument. Returns PDF doc
        public IronPdf.PdfDocument CreatePDF(List<Flower> results)
        {
            // Gets html for PDF conversion
            String htmlString = GetHtml(results);
            // Create the IronPDF obj
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();

            // Set up print options
            Renderer.PrintOptions.MarginTop = 5;
            Renderer.PrintOptions.MarginBottom = 5;
            Renderer.PrintOptions.MarginLeft = 5;  
            Renderer.PrintOptions.MarginRight = 5;
            
            IronPdf.PdfDocument PDF = Renderer.RenderHtmlAsPdf(htmlString);

            return PDF;
        }

        // Returns html string with information of flower(s). Takes Flower list as argument
        public string GetHtml(List<Flower> results)
        {
            String htmlString = "<body style='background-color: #FEEDED'>";
            // Loop through flowers
            for (int i = 0; i < results.Count; i++)
            {
                htmlString += "<div><h1 style = 'font-size: 230%; text-decoration: underline;'>" + results[i].GetEnglishName() + "</h1>";
                htmlString += "<p style='margin-left: 50px;'>Latin Name - " + results[i].GetLatinName() + "</p>";
                htmlString += "<p style='margin-left: 50px;'>Botanical Family - " + results[i].GetBotanicalFamily() + "</p><br/>";
                htmlString += "<h2 style='margin-left: 50px; text-decoration: underline; font-size: 140%'>Images</h2>";
                // Loop through flower images
                for (int j = 0; j < results[i].GetImages().Count; j++)
                {
                    htmlString += "<div style='margin-left: 70px; height: 400px; width: 400px;'><img src = '" + results[i].GetImages()[j].GetImageLocation() + "' style = 'height: 100%; width: 100%; object-fit: contain;'></div>";
                    htmlString += "<h2 style='margin-left: 90px; text-decoration: underline; font-size: 130%'>Image Note</h2>";
                    htmlString += "<p style='margin-left: 100px;text-indent: 20px;'>" + results[i].GetImages()[j].GetNote().GetInfo() + "</p><br/>";
                }
                htmlString += "<h2 style='margin-left: 50px; text-decoration: underline; font-size: 140%'>Flower Notes</h2>";
                // Loop through flower notes
                for (int j = 0; j < results[i].GetNotes().Count; j++)
                {
                    htmlString += "<p style='margin-left: 70px;text-indent: 20px;'>" + results[i].GetNotes()[j].GetInfo() + "</p><br/>";
                }
                htmlString += "</div>";
            }
            htmlString += "</body>";

            return htmlString;
        }
    }
}
