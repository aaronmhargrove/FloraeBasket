//
// FILE: TextSearchController.cs
// INFO: Link between front end GUI and back end database. Controlls text searching
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreProject
{
    public class TextSearchController
    {
        // Ctor
        public TextSearchController() { }

        // Searches database for with given string
        // Returns max of top 10 results
        public List<Flower> SearchByText(String searchString)
        {
            searchString = searchString.ToLower();

            // Check if the user is searching for a specific color flower
            if(searchString == "yellow" || searchString == "red" || searchString == "green" || searchString == "blue" ||
                searchString == "orange" || searchString == "pink" || searchString == "purple" || searchString == "white")
            {
                // Search by RGB values
                List<Flower> colorResults = SearchByColor(searchString);
                return colorResults;
            }
            // Else do a normal search
            else
            {
                List<Flower> flowers = DBMgr.GetInstance().GetFlowers();
                String[] splits = searchString.Split(' ');
                flowers = Search(searchString.ToLower(), flowers); // searches full string first for full match
                for (int i = 0; i < splits.Length; i++) // searches split string second for partial matches
                {
                    // Do not count common words as matches
                    if ((splits[i].ToLower() != "a") && (splits[i].ToLower() != "and") && (splits[i].ToLower() != "the"))
                        flowers = Search(splits[i].ToLower(), flowers);
                }

                List<Flower> results = OrderFlowers(flowers);

                return results;
            }
            
        }

        // Searhes for specific color flower using SearchByHistogramController and RGB (basic colors only for text search)
        // Takes user's search string. Returns list of matches
        public List<Flower> SearchByColor(String searchString)
        {
            switch (searchString)
            {
                case "yellow":
                    return SearchHistogramController.SearchByHistogram(216, 255, 58);
                case "red":
                    return SearchHistogramController.SearchByHistogram(255, 0, 0);
                case "green":
                    return SearchHistogramController.SearchByHistogram(0, 255, 0);
                case "blue":
                    return SearchHistogramController.SearchByHistogram(0, 0, 255);
                case "orange":
                    return SearchHistogramController.SearchByHistogram(255, 187, 67);
                case "pink":
                    return SearchHistogramController.SearchByHistogram(255, 189, 239);
                case "purple":
                    return SearchHistogramController.SearchByHistogram(213, 150, 255);
                case "white":
                    return SearchHistogramController.SearchByHistogram(240, 255, 240);
            }

            return null;
        }

        // Orders flowers in decending order based on match strength
        // Takes unordered list and returns the list ordered
        public List<Flower> OrderFlowers(List<Flower> flowers)
        {
            // Sorted list of matches
            List<Flower> sortedList = flowers.OrderBy(x => x.GetMatches()).Reverse().ToList();
            List<Flower> topResults = new List<Flower>();

            // Loops through the sorted list and takes max of top 10 matches
            for (int i = 0; i < sortedList.Count; i++)
            {
                if ((sortedList[i].GetMatches() > 0) && (topResults.Count < 10))
                {
                    topResults.Add(sortedList[i]);
                }
            }

            return topResults;
        }

        // Looks through flower objects and rates match strength
        // Takes serch string (full or partial) and list of flowers
        // Returns list for further searching or ordering
        public List<Flower> Search(String search, List<Flower> flowers)
        {
            // Loop through flowers
            for (int i = 0; i < flowers.Count; i++)
            {
                // check flower data
                if (flowers[i].GetEnglishName().ToLower().Contains(search))
                {
                    flowers[i].AddMatchVal(5);
                }
                if (flowers[i].GetLatinName().ToLower().Contains(search))
                {
                    flowers[i].AddMatchVal(5);
                }
                if (flowers[i].GetBotanicalFamily().ToLower().Contains(search))
                {
                    flowers[i].AddMatchVal(5);
                }

                // Check flower notes
                for (int j = 0; j < flowers[i].GetNotes().Count; j++)
                {
                    // Check note info
                    String[] splits = Regex.Split(flowers[i].GetNotes()[j].GetInfo(), search, RegexOptions.IgnoreCase);
                    flowers[i].AddMatchVal(splits.Length - 1);
                    // Check note date
                    if (flowers[i].GetNotes()[j].GetDate().Contains(search))
                    {
                        flowers[i].AddMatchVal(2);
                    }
                    // Check note time
                    if (flowers[i].GetNotes()[j].GetTime().Contains(search))
                    {
                        flowers[i].AddMatchVal(2);
                    }
                }

                // Check flower images
                for (int j = 0; j < flowers[i].GetImages().Count; j++)
                {
                    // Check img location
                    if (flowers[i].GetImages()[j].GetImageLocation().ToLower().Contains(search))
                    {
                        flowers[i].AddMatchVal(3);
                    }

                    // Check image note
                    // Info
                    String[] splits = Regex.Split(flowers[i].GetImages()[j].GetNote().GetInfo(), search, RegexOptions.IgnoreCase);
                    flowers[i].AddMatchVal(splits.Length - 1);
                    // Date
                    if (flowers[i].GetImages()[j].GetNote().GetDate().Contains(search))
                    {
                        flowers[i].AddMatchVal(2);
                    }
                    // Time
                    if (flowers[i].GetImages()[j].GetNote().GetTime().Contains(search))
                    {
                        flowers[i].AddMatchVal(2);
                    }
                }
            }
            return flowers;
        }

        // Checks gui input for empty string or only whitespace
        // Returns bool (valid or !valid)
        public Boolean IsInvalid(String textBoxText)
        {
            return String.IsNullOrWhiteSpace(textBoxText);
        }
    }
}

