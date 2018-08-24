//
// FILE: AddUserController.cs
// INFO: Class for controlling business logic behind adding a user to the database.
//

using System;
using System.IO;
using System.Collections.Generic;

namespace CoreProject
{
    public class AddUserController
    {
        // Method for adding a user. Takes name, username, raw password, and user type (1 = user; 2 = researcher; 3 = admin)
        // Returns boolean value to determine success or failure.
        public static bool CreateNewUser(String name, String user, String password, int type)
        {
            // Check for input in required fields
            if(name.Equals("") || user.Equals("") || password.Equals(""))
            {
                return false;
            }

            User newUser = new User(name, user, password, type);

            // Save the new user
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.SaveNewUser(newUser);

            // Return success or failure.
            if(success) { return true; }
            else { return false; }
        }

        // Adds batch of users. Takes string filepath to csv file and returns list of added users
        public static List<String> BatchImport(String filePath)
        {
            // Create a list of users that are imported so that it can be reported back to the user.
            List<String> addedUsers = new List<String>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                string[] delim;

                // Iterate through the file line by line, adding a new user for each line
                while (((line = reader.ReadLine()) != null))
                {
                    try
                    {
                        // Split the line by ,
                        delim = line.Split(',');
                        String newName = delim[0];
                        String newUser = delim[1];
                        String newPass = delim[2];
                        int newType = Convert.ToInt32(delim[3]);
                        // Pass the new user to the standard createNewUser method to handle the rest
                        CreateNewUser(newName, newUser, newPass, newType);
                        // Add the user's name to the output list
                        addedUsers.Add(newName);
                    }
                    catch (Exception c)
                    {
                        addedUsers.Add("--Batch Import File Error");
                        addedUsers.Add("--Check file format. See info for help.");
                    }
                }
            }
            return addedUsers;
        }
    }
}
