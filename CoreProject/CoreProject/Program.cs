//
// FILE: Program.cs
// INFO: Entry point for application. Starts the program, the rest is handled by FloraeBasketGUI and controllers
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FloraeBasketGUI());
        }
    }
}
