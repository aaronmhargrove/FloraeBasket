//
// FILE: Note.cs
// INFO: Data class for note object
// LAST CHANGE: 4/5/18 11:15PM
//

using System;

namespace CoreProject
{
    public class Note
    {
        // Class data members
        private String info;
        private String date;
        private String time;

        // Ctor (information for note)
        public Note(String information)
        {
            this.info = information;
            this.time = DateTime.Now.ToString("h:mm tt");
            this.date = DateTime.Today.ToString("MM/dd/yyyy");
        }

        // DB Retrevial Ctor (information, creation date, creation time)
        public Note(String information, String date, String time)
        {
            this.info = information;
            this.date = date;
            this.time = time;
        }

        // Getters
        public String GetInfo() { return this.info; }
        public String GetDate() { return this.date; }
        public String GetTime() { return this.time; }
    }
}
