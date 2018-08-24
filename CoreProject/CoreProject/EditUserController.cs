//
// FILE: EditUserController.cs
// INFO: Class for controlling business logic behind editing an existing user
//

using System;
using System.Collections.Generic;

namespace CoreProject
{
    public class EditUserController
    {
        // Updates user info and tells DBMgr to update the database
        // Takes name, username, password, user type, and the user object being edited
        // Returns bool success or failure
        public static bool EditUser(String name, String user, String pwd, int type, User u)
        {
            // Check for edited fields
            if (!name.Equals(u.GetName()))
            {
                u.SetName(name);
            }
            if (!user.Equals(u.GetUser()))
            {
                u.SetUser(user);
            }
            if (!pwd.Equals(""))
            {
                u.SetPwd(pwd);
            }
            if (type != u.GetUserType())
            {
                u.SetType(type);
            }
            // Tell DBMgr to add the flower to the database
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.EditUser(u);
            // Determine success or failure and return
            if (success) { return true; }
            else { return false; }
        }
        
        // Tells DBMgr to delete specific user from DB
        // Takes user object as argument
        public static bool DeleteUser(User u)
        {
            DBMgr mgr = DBMgr.GetInstance();
            bool success = mgr.DeleteUser(u);
            if (success) { return true; }
            else { return false; }
        }

        // Returns a list of all users from database
        public static List<User> GetUsers()
        {
            DBMgr mgr = DBMgr.GetInstance();
            return mgr.GetUsers();
        }
    }
}
