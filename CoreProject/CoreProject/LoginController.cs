//
// FILE: LoginController.cs
// INFO: Contoller for login. Link between front end and back end.
//

using System;

namespace CoreProject
{
    public class LoginController
    {
        // ctor
        public LoginController() { }

        // Salt and hash password
        // Takes plain text password and a 10 character long salt string
        // Returns salted and hashed password
        public int SaltAndHash(String pass, String salt)
        {
            String saltedPass = pass + salt;
            int SAHPass = saltedPass.GetHashCode();

            return SAHPass;
        }
        
        // Checks for empty string or only white space
        // Takes text and returns true or false
        public Boolean IsValid(String checkMe)
        {
            if (String.IsNullOrWhiteSpace(checkMe))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Checks DB for matched values
        // Returns user if a match is found in DBMgr
        public User IsMatch(String providedUsername, String providedPassword)
        {
            User user = DBMgr.GetInstance().IsMatch(providedUsername, providedPassword);
            return user;
        }
    }
}
