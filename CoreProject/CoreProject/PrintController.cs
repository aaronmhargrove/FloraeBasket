//
// FILE: PrintController.cs
// INFO: Controller for printing search results (uses PdfController)
// DEPENDENT ON: Adobe Acrobat. Acrobat must be set as default pdf viewer for pdf printing to work.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CoreProject
{
    public class PrintController
    {
        // Ctor
        public PrintController() { }

        // Starts print process (OS) and opens print dialog
        // Takes list of Flower objects
        public void PrintResults(List<Flower> results)
        {
            PdfController ctrl = new PdfController();
            IronPdf.PdfDocument PDF = ctrl.CreatePDF(results);
            String filepath = SaveTemp(PDF);

            // Use of Adobe Acrobat to print a pdf (required)
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.ShowDialog();

                // Sets up the print process
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    Verb = "Print",
                    CreateNoWindow = true,
                    FileName = filepath,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                try
                {
                    process.Start();
                    process.CloseMainWindow();
                    process.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        // Saves temp copy of pdf in bin file so that it can be sent to the printer
        // Returns location (string)
        public String SaveTemp(IronPdf.PdfDocument PDF)
        {
            // Describes the path to a tempPdf folder in the project's bin file
            string appPath = Path.GetDirectoryName(Application.StartupPath) + @"\tempPdf\";

            System.IO.Directory.CreateDirectory(appPath);

            // Save the pdf to the tempPdf folder
            String filepath = appPath + "printable.pdf";
            PDF.SaveAs(filepath);

            return filepath;
        }
    }
}
