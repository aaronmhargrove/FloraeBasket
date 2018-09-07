//
// FILE: DBMgr.cs
// INFO: Class for handling all interactions with the database
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace CoreProject
{
    // Singleton
    public class DBMgr
    {
        private static DBMgr instance; 
        private static SqlConnection connection = new SqlConnection();

        // Ctor ( called by getInstance() )
        private DBMgr() { }

        // gets current instance or creates new one if one does not exist
        public static DBMgr GetInstance()
        {
            if (instance == null) {
                instance = new DBMgr();
                InitializeConnection();
            }
            return instance;
        }

        // Initializes connection to mdf file
        private static void InitializeConnection()
        {
            connection.ConnectionString = @"Data Source = (localDB)\MSSQLLocalDB;" +
                 @"AttachDbFilename = |DataDirectory|\FloraeBasket.mdf;" +
                 "Integrated Security = True;" +
                 "MultipleActiveResultSets = True;" +
                 "Connect Timeout = 30"
             ;
        }


        //
        //
        // USE CASE METHODS
        //
        //


        // Gets list of users from DB (for viewing, editing, and deletion by admin)
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            SqlCommand getUsers = new SqlCommand("SELECT * FROM Users", connection);

            connection.Open();

            SqlDataReader reader = getUsers.ExecuteReader();

            // If there are users
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Build user object
                    User addMe = new User(reader.GetString(3), reader.GetString(1), "Password Hidden" , reader.GetInt32(4));
                    addMe.SetId(reader.GetInt32(0));
                    addMe.SetSalt(reader.GetString(5));
                    users.Add(addMe);
                }
            }
            connection.Close();

            return users;
        }

        
        // Gets data for histograms and returns histogram objects (for graph creation and image comparison)
        public List<Histogram> GetHistograms()
        {
            var histId = 0;
            int[] RBin;
            int[] GBin;
            int[] BBin;
            var flowerImageId = 0;
            var flowerId = 0;
            List<Histogram> histograms = new List<Histogram>();
            SqlCommand command = new SqlCommand("SELECT * FROM Histograms", connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            // If there are histograms
            if (reader.HasRows)
            {
                // Loop through list to get RGB for each pixel
                while (reader.Read())
                {
                    histId = reader.GetInt32(0);
                    RBin = reader.GetString(1).Split(',').Select(x => int.Parse(x)).ToArray();
                    GBin = reader.GetString(2).Split(',').Select(x => int.Parse(x)).ToArray();
                    BBin = reader.GetString(3).Split(',').Select(x => int.Parse(x)).ToArray();
                    flowerImageId = reader.GetInt32(4);
                    flowerId = reader.GetInt32(5);
                    
                    // Create histogram with values from database
                    Histogram hist = new Histogram(histId, RBin, GBin, BBin, flowerId, flowerImageId);
                    histograms.Add(hist);
                }
            }
            reader.Close();

            connection.Close();

            return histograms;
        }

        // Takes histogram as argument. 
        // Returns flower object that matches the histogram
        public Flower GetFlowerByHistogram(Histogram hist)
        {
            int flowerId;
            String englishName;
            String latinName;
            String botanicalFamily;
            Flower flower;
            List<FlowerImage> pictures = new List<FlowerImage>();
            List<Note> notes = new List<Note>();

            // Select flower by flowerId
            SqlCommand flowerCommand = new SqlCommand("SELECT * FROM Flowers WHERE Id = @flowerId", connection);

            flowerCommand.Parameters.Add("@flowerId", System.Data.SqlDbType.Int);
            flowerCommand.Parameters["@flowerId"].Value = hist.GetFlowerId();

            connection.Open();
            
            using (SqlDataReader flowerReader = flowerCommand.ExecuteReader())
            {
                // Fetch all availible histogram data
                if (flowerReader.HasRows)
                {
                    while (flowerReader.Read())
                    {
                        // Get all flower data
                        flowerId = flowerReader.GetInt32(0);
                        englishName = flowerReader.GetString(1);
                        latinName = flowerReader.GetString(2);
                        botanicalFamily = flowerReader.GetString(3);
                        pictures = GetImagesForFlower(hist.GetFlowerId());
                        notes = GetNotesForFlower(hist.GetFlowerId());

                        // Build flower object
                        flower = new Flower(flowerId, englishName, latinName, botanicalFamily, notes, pictures);

                        connection.Close();

                        return flower;
                    }
                }
            }
            connection.Close();

            return null;
        }

        // Takes flower ID as argument
        // Returns images for that flower
        public List<FlowerImage> GetImagesForFlower(int flowerId)
        {
            List<FlowerImage> flowerImages = new List<FlowerImage>();
            List<Note> imageNotes = new List<Note>();
            SqlCommand imagesCommand = new SqlCommand("SELECT * FROM FlowerImages WHERE FlowerId = @flowerId", connection);
            imagesCommand.Parameters.Add("@flowerId", System.Data.SqlDbType.Int);
            imagesCommand.Parameters["@flowerId"].Value = flowerId;
            
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // Create dataset to hold result 
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(imagesCommand);
            da.Fill(ds, "FlowerImages");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                // Get image location
                int id = Int32.Parse(ds.Tables[0].Rows[i]["Id"].ToString());
                String imageLocation = ds.Tables[0].Rows[i]["ImageLocation"].ToString();

                // Get image notes
                imageNotes = GetNotesForImage(id);

                // Construct image object to return
                FlowerImage flowerImage = new FlowerImage(id, imageLocation, imageNotes.First(), flowerId);
                flowerImages.Add(flowerImage);
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            return flowerImages;
        }

        // Takes an image ID
        // Returns notes for a that image
        public List<Note> GetNotesForImage(int imageId)
        {
            List<Note> notes = new List<Note>();
            SqlCommand notesCommand = new SqlCommand("SELECT * FROM Notes WHERE FlowerImageId = @imageId", connection);
            notesCommand.Parameters.Add("@imageId", System.Data.SqlDbType.Int);
            notesCommand.Parameters["@imageId"].Value = imageId;

            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            using (SqlDataReader notesReader = notesCommand.ExecuteReader())
            {
                // Fetch all avalible note data
                if (notesReader.HasRows)
                {
                    while (notesReader.Read())
                    {
                        // Get note data
                        int id = notesReader.GetInt32(0);
                        String date = notesReader.GetDateTime(1).ToString();
                        String info = notesReader.GetString(2);
                        String time = notesReader.GetTimeSpan(5).ToString();

                        // Constuct note object to return
                        Note note = new Note(info, date, time);
                        notes.Add(note);
                    }
                }
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            return notes;
        }

        // Takes flower ID as argument
        // Returns notes for that flower
        public List<Note> GetNotesForFlower(int flowerId)
        {
            List<Note> notes = new List<Note>();
            SqlCommand notesCommand = new SqlCommand("SELECT * FROM Notes WHERE FlowerId = @flowerId", connection);
            notesCommand.Parameters.Add("@flowerId", System.Data.SqlDbType.Int);
            notesCommand.Parameters["@flowerId"].Value = flowerId;

            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            using (SqlDataReader notesReader = notesCommand.ExecuteReader())
            {
                // Fetch all avalible note data
                if (notesReader.HasRows)
                {
                    while (notesReader.Read())
                    {
                        // Get note data
                        int id = notesReader.GetInt32(0);
                        String date = notesReader.GetDateTime(1).ToString();
                        String info = notesReader.GetString(2);
                        String time = notesReader.GetTimeSpan(5).ToString();

                        // Construct note objects for return
                        Note note = new Note(info, date, time);
                        notes.Add(note);
                    }
                }
            }

            return notes;
        }

        // Used during login. Matches provided data against data in DB
        // Takes user provided username and password as arguments
        // Returns a matched user if there is a match
        public User IsMatch(String user, String pass)
        {
            // check database for match and return user if there is one, else return null
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @username");
            cmd.Parameters.AddWithValue("@username", user);
            cmd.Connection = connection;
            connection.Open();

            // Create dataset to hold result 
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Users");

            // If the username does not exist
            if(ds.Tables[0].Rows.Count == 0)
            {
                connection.Close();
                return null;
            }
            // If the username does
            else
            {
                // Get password
                DataRow dr = ds.Tables[0].Rows[0];
                LoginController ctrl = new LoginController();
                int SHP = ctrl.SaltAndHash(pass, dr["Salt"].ToString());
                
                // If the password matches
                if (Int32.Parse((dr["Password"]).ToString()) == SHP)
                {
                    User usr = new User(dr["Name"].ToString(), "0000", "0000", Int32.Parse(dr["Type"].ToString()));
                    connection.Close();
                    return usr;
                }
                else
                {
                    connection.Close();
                    return null;
                }
            }
        } // end IsMatch()

        // Get all flower entries from the database and return to controller
        public List<Flower> GetFlowers()
        {
            connection.Open();

            // Gets Flower Table
            SqlCommand flowerCmd = new SqlCommand("SELECT * FROM Flowers;");
            flowerCmd.Connection = connection;
            DataSet FlowerDS = new DataSet();
            SqlDataAdapter FlowerDA = new SqlDataAdapter(flowerCmd);
            FlowerDA.Fill(FlowerDS, "Flowers");

            List<Flower> flowers = new List<Flower>();

            // Loop through flowers
            for (int f = 0; f < FlowerDS.Tables[0].Rows.Count; f++)
            {
                int currentFlowerID = Int32.Parse((FlowerDS.Tables[0].Rows[f]["id"]).ToString());
                String engName = (FlowerDS.Tables[0].Rows[f]["EnglishName"]).ToString();
                String latName = (FlowerDS.Tables[0].Rows[f]["LatinName"]).ToString();
                String botFamily = (FlowerDS.Tables[0].Rows[f]["BotanicalFamily"]).ToString();
                List<FlowerImage> flowerImages = GetImagesForFlower(currentFlowerID);
                List<Note> flowerNotes = GetNotesForFlower(currentFlowerID);

                // Create flower and add to results list
                Flower flower = new Flower(engName, latName, botFamily, flowerNotes, flowerImages);
                //flower.SetId(currentFlowerID);
                flowers.Add(flower);
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            return flowers;

        } // end GetFlowers()

        // Called by AddFlowerController to save a new flower in the database.
        // Takes a flower as an argument
        // Returns boolean to indicate success or failure
        public bool SaveNewFlower(Flower newFlower)
        {
            // Prepare SQL command strings
            String sqlFlower = "INSERT INTO Flowers (EnglishName, LatinName, BotanicalFamily) VALUES (@engName, @latName, @family)";
            String sqlLastFlowerID = "SELECT IDENT_CURRENT('Flowers')";
            String sqlFlowerNote = "INSERT INTO Notes (Date, Info, FlowerId, Time) VALUES (@date, @info, @flowerid, @time)";
            String sqlImageNote = "INSERT INTO Notes (Date, Info, FlowerImageId, Time) VALUES (@date, @info, @imgid, @time)";
            String sqlLastNoteID = "SELECT IDENT_CURRENT('Notes')";
            String sqlFlowerImages = "INSERT INTO FlowerImages (ImageLocation, FlowerId) VALUES (@location, @flowerid)";
            String sqlLastFlowerImagesID = "SELECT IDENT_CURRENT('FlowerImages')";
            String sqlHistogram = "INSERT INTO Histograms (RBin, GBin, BBin, FlowerImageId, FlowerId) VALUES (@r, @g, @b, @flowerid, @imgid)";

            // Parameter binding for inserFlower command
            SqlCommand insertFlower = new SqlCommand(sqlFlower, connection);

            insertFlower.Parameters.Add("@engName", System.Data.SqlDbType.VarChar);
            insertFlower.Parameters.Add("@latName", System.Data.SqlDbType.VarChar);
            insertFlower.Parameters.Add("@family", System.Data.SqlDbType.VarChar);

            insertFlower.Parameters["@engName"].Value = newFlower.GetEnglishName();
            insertFlower.Parameters["@latName"].Value = newFlower.GetLatinName();
            insertFlower.Parameters["@family"].Value = newFlower.GetBotanicalFamily();

            // Parameter binding for insertNote command
            SqlCommand insertFlowerNote = new SqlCommand(sqlFlowerNote, connection);
            SqlCommand insertImgNote = new SqlCommand(sqlImageNote, connection);

            insertFlowerNote.Parameters.Add("@date", System.Data.SqlDbType.VarChar);
            insertFlowerNote.Parameters.Add("@info", System.Data.SqlDbType.VarChar);
            insertFlowerNote.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            insertFlowerNote.Parameters.Add("@time", System.Data.SqlDbType.VarChar);

            insertImgNote.Parameters.Add("@date", System.Data.SqlDbType.VarChar);
            insertImgNote.Parameters.Add("@info", System.Data.SqlDbType.VarChar);
            insertImgNote.Parameters.Add("@imgid", System.Data.SqlDbType.Int);
            insertImgNote.Parameters.Add("@time", System.Data.SqlDbType.VarChar);

            // Parameter binding for insertFlowerImages command
            SqlCommand insertFlowerImages = new SqlCommand(sqlFlowerImages, connection);

            insertFlowerImages.Parameters.Add("@location", System.Data.SqlDbType.VarChar);
            insertFlowerImages.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);

            // Parameter binding for insertHistogram command
            SqlCommand insertHistogram = new SqlCommand(sqlHistogram, connection);

            insertHistogram.Parameters.Add("@r", System.Data.SqlDbType.Int);
            insertHistogram.Parameters.Add("@g", System.Data.SqlDbType.Int);
            insertHistogram.Parameters.Add("@b", System.Data.SqlDbType.Int);
            insertHistogram.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            insertHistogram.Parameters.Add("@imgid", System.Data.SqlDbType.Int);

            // Commands for retrieving the last ID inserted into the database according to their tables
            SqlCommand getLastFlowerID = new SqlCommand(sqlLastFlowerID, connection);
            SqlCommand getLastFlowerImagesID = new SqlCommand(sqlLastFlowerImagesID, connection);
            SqlCommand getLastNoteID = new SqlCommand(sqlLastNoteID, connection);


            // Insert the flower into the flowers table
            connection.Open();
            insertFlower.ExecuteNonQuery();
            connection.Close();

            // Get the id of the last inserted flower to use as a foreign key for notes, images, and histograms
            connection.Open();
            SqlDataReader lastFlowerIdReader = getLastFlowerID.ExecuteReader();
            int lastFlower;
            if (lastFlowerIdReader.Read())
            {
                lastFlower = Int32.Parse(String.Format("{0}", lastFlowerIdReader[0]));
                connection.Close();
            }
            else
            {
                connection.Close();
                return false;
            }

            // Insert the notes associated with the value 
            for(int i = 0; i < newFlower.GetNotes().Count; i++)
            {
            insertFlowerNote.Parameters["@date"].Value = newFlower.GetNote(i).GetDate();
            insertFlowerNote.Parameters["@info"].Value = newFlower.GetNote(i).GetInfo();
            insertFlowerNote.Parameters["@flowerid"].Value = lastFlower;
            insertFlowerNote.Parameters["@time"].Value = newFlower.GetNote(i).GetTime();
            connection.Open();
            insertFlowerNote.ExecuteNonQuery();
            connection.Close();
            }

            // Insert all images associated with the flower, along with their respective notes and histogram data.
            if (newFlower.GetImages() != null)
            {
                List<FlowerImage> images = newFlower.GetImages();
                for (int i = 0; i < images.Count; i++)
                {
                    // Insert the image at index i
                    insertFlowerImages.Parameters["@location"].Value = images[i].GetImageLocation();
                    insertFlowerImages.Parameters["@flowerid"].Value = lastFlower;

                    connection.Open();
                    insertFlowerImages.ExecuteNonQuery();
                    connection.Close();

                    // Get the id of the last image inserted 
                    connection.Open();
                    SqlDataReader lastFlowerImgIdReader = getLastFlowerImagesID.ExecuteReader();
                    int lastFlowerImg;
                    if (lastFlowerImgIdReader.Read())
                    {
                        lastFlowerImg = Int32.Parse(String.Format("{0}", lastFlowerImgIdReader[0]));
                        connection.Close();
                    }
                    else
                    {
                        connection.Close();
                        return false;
                    }

                    images[i].SetId(lastFlowerImg);
                    images[i].SetFlowerId(lastFlower);

                    // Save histogram to database
                    images[i].SetHistogramId(SaveHistogram(images[i]));

                    // Insert the note belonging to image at index i
                    insertImgNote.Parameters["@date"].Value = images[i].GetNote().GetDate();
                    insertImgNote.Parameters["@info"].Value = images[i].GetNote().GetInfo();
                    insertImgNote.Parameters["@imgid"].Value = lastFlowerImg;
                    insertImgNote.Parameters["@time"].Value = images[i].GetNote().GetTime();

                    connection.Open();
                    insertImgNote.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return true;
        }

        // Called by EditFlowerController to edit an existing flower in the database
        // Takes a flower object as anargument
        public bool EditFlower(Flower editFlower)
        {
            // Save the edited flower as a new flower in the database
            SaveNewFlower(editFlower);
            // Delete the old flower and all items with relationships to it
            DeleteFlower(editFlower);

            return true;
        }

        // Called by EditFlowerController to remove a flower from the database
        // Takes flower object as argument
        public bool DeleteFlower(Flower deleteFlower)
        {
            // Prepare SQL command strings
            String sqlDeleteFlower = "DELETE FROM Flowers WHERE id = @flowerid";
            String sqlDeleteFlowerNotes = "DELETE FROM Notes WHERE FlowerId = @flowerid";
            String sqlDeleteImageNotes = "DELETE FROM Notes WHERE FlowerImageId = @flowerimgid";
            String sqlDeleteImages = "DELETE FROM FlowerImages WHERE FlowerId = @flowerid";
            String sqlSelectImageIds = "SELECT Id FROM FlowerImages WHERE FlowerId = @flowerid";
            String sqlDeleteHistograms = "DELETE FROM Histograms WHERE FlowerId = @flowerid";

            SqlCommand flowerDelete = new SqlCommand(sqlDeleteFlower, connection);
            SqlCommand flowerNotesDelete = new SqlCommand(sqlDeleteFlowerNotes, connection);
            SqlCommand flowerImageNotesDelete = new SqlCommand(sqlDeleteImageNotes, connection);
            SqlCommand imagesDelete = new SqlCommand(sqlDeleteImages, connection);
            SqlCommand getImageIds = new SqlCommand(sqlSelectImageIds, connection);
            SqlCommand deleteHistograms = new SqlCommand(sqlDeleteHistograms, connection);

            // Bind parameters to their commands
            flowerDelete.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            flowerNotesDelete.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            flowerImageNotesDelete.Parameters.Add("@flowerimgid", System.Data.SqlDbType.Int);
            imagesDelete.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            getImageIds.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);
            deleteHistograms.Parameters.Add("@flowerid", System.Data.SqlDbType.Int);

            // Set the parameters to the id of the flower being removed
            flowerDelete.Parameters["@flowerid"].Value = deleteFlower.GetId();
            flowerNotesDelete.Parameters["@flowerid"].Value = deleteFlower.GetId();
            imagesDelete.Parameters["@flowerid"].Value = deleteFlower.GetId();
            getImageIds.Parameters["@flowerid"].Value = deleteFlower.GetId();
            deleteHistograms.Parameters["@flowerid"].Value = deleteFlower.GetId();

            // Delete all histograms related to the flower being removed
            connection.Open();
            deleteHistograms.ExecuteNonQuery();
            connection.Close();

            // For each image, get the image id so that the associated notes can be removed
            // imageIdList will hold a list of all images associated with the flower
            List<int> imageIdList = new List<int>();
            connection.Open();
            SqlDataReader imageIdReader = getImageIds.ExecuteReader();
            while (imageIdReader.Read())
            {
                imageIdList.Add(Int32.Parse(String.Format("{0}", imageIdReader[0])));
            }
            connection.Close();

            // For each image ID, delete notes associated with that image.
            foreach(int imageId in imageIdList)
            {
                flowerImageNotesDelete.Parameters["@flowerimgid"].Value = imageId;
                connection.Open();
                flowerImageNotesDelete.ExecuteNonQuery();
                connection.Close();
            }

            // After deleting all notes associated with images, safely delete images
            connection.Open();
            imagesDelete.ExecuteNonQuery();
            connection.Close();

            // Delete any notes directly associated with the flower
            connection.Open();
            flowerNotesDelete.ExecuteNonQuery();
            connection.Close();

            // Finally, delete the flower after all of its related objects have been deleted
            connection.Open();
            flowerDelete.ExecuteNonQuery();
            connection.Close();

            return true;
        }

        // Called by EditUserController
        // Takes a user as an argument for edit
        public bool EditUser(User u)
        {  
            SaveNewUser(u);
            DeleteUser(u);

            return true;
        }

        // Called by EditUserController and EditUser method to delete a user from the database
        // Takes a user as an argument for deletion
        public bool DeleteUser(User u)
        {
            String sql = "DELETE FROM Users WHERE Id = @userid";
            SqlCommand deleteUser = new SqlCommand(sql, connection);

            deleteUser.Parameters.Add("@userid", System.Data.SqlDbType.Int);
            deleteUser.Parameters["@userid"].Value = u.GetId();

            connection.Open();

            // Delete the selected user
            deleteUser.ExecuteNonQuery();

            connection.Close();

            return true;
        }

        // Called by AddUserController to save a new user in the database.
        // Takes a user as an argument
        public bool SaveNewUser(User newUser)
        {
            SqlCommand command;
            String sql;

            sql = "INSERT INTO Users (Username, Password, Name, Type, Salt) VALUES (@username, @password, @name, @type, @salt)";

            command = new SqlCommand(sql, connection);

            // Prepare SQL
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@password", System.Data.SqlDbType.Int);
            command.Parameters.Add("@name", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@type", System.Data.SqlDbType.Int);
            command.Parameters.Add("@salt", System.Data.SqlDbType.VarChar);
            command.Parameters["@username"].Value = newUser.GetUser();
            command.Parameters["@password"].Value = newUser.GetPwd();
            command.Parameters["@name"].Value = newUser.GetName();
            command.Parameters["@type"].Value = newUser.GetUserType();
            command.Parameters["@salt"].Value = newUser.GetSalt();

            connection.Open();

            // Add user
            command.ExecuteNonQuery();

            connection.Close();
            
            return true;
        }

        // Saves flower image to database (location, not image itself)
        // Takes a FlowerImage object as an argument
        public int SaveFlowerImage(FlowerImage flowerImage)
        {
            SqlCommand command;
            String sql;
            int newId = 0;

            sql = "INSERT INTO FlowerImages (ImageLocation, FlowerId) OUTPUT INSERTED.ID VALUES (@location, @flowerId)";

            command = new SqlCommand(sql, connection);

            // Prepare parameters
            command.Parameters.Add("@location", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@flowerId", System.Data.SqlDbType.VarChar);
            command.Parameters["@location"].Value = flowerImage.GetImageLocation();
            command.Parameters["@flowerId"].Value = 1;

            try
            {
                connection.Open();

                // Save image
                newId = Int32.Parse(command.ExecuteScalar().ToString());
                connection.Close();
                flowerImage.SetId(newId);
                flowerImage.SetHistogramId(SaveHistogram(flowerImage));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return newId;
        }

        // Saves histogram data to the database
        public int SaveHistogram(FlowerImage flowerImage)
        {
            SqlCommand command;
            String sql;
            int newId = 0;

            Histogram hist = flowerImage.GetHistogram();
            
            String RBin = string.Join(",", hist.GetRBin());
            String GBin = string.Join(",", hist.GetGBin());
            String BBin = string.Join(",", hist.GetBBin());

            sql = "INSERT INTO Histograms (RBin, GBin, BBin, FlowerImageId, FlowerId) OUTPUT INSERTED.ID VALUES (@RBin, @GBin, @BBin, @FlowerImageId, @FlowerId)";

            command = new SqlCommand(sql, connection);

            // Prepare parameters
            command.Parameters.Add("@RBin", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@GBin", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@BBin", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@FlowerImageId", System.Data.SqlDbType.VarChar);
            command.Parameters.Add("@FlowerId", System.Data.SqlDbType.VarChar);
            command.Parameters["@RBin"].Value = RBin;
            command.Parameters["@GBin"].Value = GBin;
            command.Parameters["@BBin"].Value = BBin;
            command.Parameters["@FlowerImageId"].Value = flowerImage.GetId();
            command.Parameters["@FlowerId"].Value = flowerImage.GetFlowerId();

            connection.Open();
            
            try
            {
                // Save histogram data
                newId = Int32.Parse(command.ExecuteScalar().ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            connection.Close();

            return newId;
        }
    }
}

