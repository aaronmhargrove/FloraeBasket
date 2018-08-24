//
// FILE: User.cs
// INFO: Data class for user object
//

using System;

namespace CoreProject
{
    public class User
    {
        // Data members
        private String name;
        private String user;
        private int pwd;
        private int type;
        private int id;
        private String salt; 
  
        // Ctor
        // Name, username, password, type (1 = user, 2 = researcher, 3 = admin)
        public User(String name, String user, String pwd, int type)
        {
            this.name = name;
            this.user = user;
            this.type = type;
            this.salt = SecurityController.GenerateSalt();
            this.pwd = SecurityController.GenerateHash(pwd, this.salt);
        }

        // Returns name and username
        public override string ToString()
        {
            return name + " (" + user + ")";
        }

        // Getters
        public String GetName() { return this.name; }
        public String GetUser() { return this.user; }
        public int GetPwd() { return this.pwd; }
        public int GetUserType() { return this.type; }
        public String GetSalt() { return this.salt; }
        public int GetId() { return this.id; }

        // Setters
        public void SetName(String newName) { this.name = newName; }
        public void SetUser(String newUser) { this.user = newUser; }
        public void SetPwd(String newPwd) { this.pwd = SecurityController.GenerateHash(newPwd, this.salt); }
        public void SetType(int newType) { this.type = newType; }
        public void SetSalt(String salt) { this.salt = salt; }
        public void SetId(int id) { this.id = id; }
    }
}
