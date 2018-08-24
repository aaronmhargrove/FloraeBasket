//
// FILE: SecurityController.cs
// INFO: Class for controlling the salting and hashing for passwords.
//

using System;
using System.Security.Cryptography;

namespace CoreProject
{
    public class SecurityController
    {
        // Returns salted and hashed password for storing in Database.
        // Takes plain text password and 10 character long salt string.
        public static int GenerateHash(String password, String salt)
        {
            String hashString = password + salt;
            return (hashString.GetHashCode());
        }

        // Compares an existing hash password in the database with a password entered by a user
        // Takes password, salt string, and exsiting hash. Returns bool (match or !match)
        public static bool ValidatePassword(String password, String salt, int existingHash)
        {
            return (GenerateHash(password, salt) == existingHash);
        }

        // Returns a random 10 character string to be used as a salt
        public static String GenerateSalt()
        {
            byte[] newSalt;
            new RNGCryptoServiceProvider().GetBytes(newSalt = new byte[7]);
            String saltString = Convert.ToBase64String(newSalt);
            saltString = saltString.Substring(0, 10);
            return saltString;
        }
    }
}
