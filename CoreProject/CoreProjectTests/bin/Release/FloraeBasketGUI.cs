//
// FILE: FloraeBasket.cs
// INFO: Control for program. Contains all listeners for GUI
// LAST CHANGE: 7/11/18 1:41pm
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CoreProject
{
    public partial class FloraeBasketGUI : Form
    {
        // Variables
        private String user_Name;
        private int user_Type;
        private List<Flower> SBTRResults = new List<Flower>();
        private List<FlowerImage> SBTRImgs = new List<FlowerImage>();
        private List<FlowerImage> editFlowerImages = new List<FlowerImage>();
        private int editFlowerPic = 0;
        private Histogram selectedHistogram;
        private List<Note> SBTRNotes = new List<Note>();
        private int SBTRNumPics = 0, SBTRNumFlowers = 0, SBTRNumNotes = 0, SBTRCurrentPicture = 0, SBTRCurrentFlower = 0, SBTRCurrentNote = 0;
        private List<FlowerImage> importedPhotos;

        public FloraeBasketGUI()
        {
            InitializeComponent();
            ClearAll();
            SizeAndPositionPanels();
            LoginPanel.Size = new Size(1299, 708);
            LoginPanel.Visible = true;
        }

        // Hides all panels. First line of button listener should call this and then show the proper panel 
        public void ClearAll()
        {
            SBTPanel.Visible = false;
            SBTRPanel.Visible = false;
            searchHistogramPanel.Hide();
            searchImagePanel.Visible = false;
            addFlowerPanel.Hide();
            addUserPanel.Hide();
            LoginPanel.Visible = false;
            editFlowerPanel.Hide();
            SBTPanel.Visible = false;
            SBTRPanel.Visible = false;
            EditUserPanel.Hide();
            viewHistogramPanel.Visible = false;
        }

        // Initilizes panels to correct position
        public void SizeAndPositionPanels()
        {
            Size size = new Size(1184, 600);
            Point location = new Point(112, 107);

            // Size panels
            SBTPanel.Size = size;
            SBTRPanel.Size = size;
            searchHistogramPanel.Size = size;
            searchImagePanel.Size = size;
            addFlowerPanel.Size = size;
            addUserPanel.Size = size;
            editFlowerPanel.Size = size;
            SBTPanel.Size = size;
            SBTRPanel.Size = size;
            // Position panels
            SBTPanel.Location = location;
            SBTRPanel.Location = location;
            searchHistogramPanel.Location = location;
            searchImagePanel.Location = location;
            addFlowerPanel.Location = location;
            addUserPanel.Location = location;
            editFlowerPanel.Location = location;
            SBTPanel.Location = location;
            SBTRPanel.Location = location;
        }



        //
        //
        // LOGOUT
        //
        //

        // Listener for logout button on main menu -> Returns to login page
        private void logoutBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            LoginPanel.Visible = true;
            LoginErrorLabel.Visible = false;
            LoginUsernameTB.Text = "";
            LoginPasswordTB.Text = "";
        }

        
        
        //
        //
        // LOGIN
        //
        //

        // Listener for login button on login panel -> Uses LoginController to validate credentials
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Login Controller
            LoginController ctrl = new LoginController();

            // Validates that there is text in the input fields
            if (ctrl.IsValid(LoginUsernameTB.Text) && ctrl.IsValid(LoginPasswordTB.Text)) 
            {
                // Go to controler->DB to check for matched user
                User user = ctrl.IsMatch(LoginUsernameTB.Text, LoginPasswordTB.Text);

                // If login failed, display error. Else, customize display and display home screen
                if (user == null)
                {
                    LoginErrorLabel.Visible = true;
                }
                else
                {   
                    // Set current user info for customized display
                    this.user_Name = user.GetName();
                    this.user_Type = user.GetUserType();

                    if(user_Type == 1)      // User
                    {
                        addFlowerBttn.Visible = false;
                        editFlowerBttn.Visible = false;
                        addUserBttn.Visible = false;
                        editUserBttn.Visible = false;
                        viewHistogramBttn.Visible = false;
                    }
                    else if(user_Type == 2) // Researcher
                    {
                        addFlowerBttn.Visible = true;
                        editFlowerBttn.Visible = true;
                        addUserBttn.Visible = false;
                        editUserBttn.Visible = false;
                        viewHistogramBttn.Visible = true;
                    }
                    else                    // Admin
                    {
                        addFlowerBttn.Visible = true;
                        editFlowerBttn.Visible = true;
                        addUserBttn.Visible = true;
                        editUserBttn.Visible = true;
                        viewHistogramBttn.Visible = true;
                    }
                    ClearAll();
                    SBTPanel.Visible = true;
                    SBTError.Visible = false;
                    SBTSearchBar.Text = "";
                }
            }
            else
            {
                LoginErrorLabel.Visible = true;
            }
        }



        //
        //
        // SEARCH BY TEXT
        //
        //

        // Listener for search by text button on main menu -> Displays search by text panel -- clears errors and searh bar
        private void searchTextBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            SBTPanel.Visible = true;
            SBTError.Visible = false;
            SBTSearchBar.Text = "";
        }

        // Uses TextSearchController to search database with text from the user
        private void SBTSearchBttn_Click(object sender, EventArgs e)
        {
            TextSearchController ctrl = new TextSearchController();
            // Validate that the search bar has valid text in it
            if (ctrl.IsInvalid(SBTSearchBar.Text))
            {
                SBTError.Visible = true;
            }
            else
            {
                // Display results to the user
                DisplayResults(ctrl.SearchByText(SBTSearchBar.Text));
            }
        }

        // Displays text search results to the user
        private void DisplayResults(List<Flower> results)
        {
            // Set up panel
            ClearAll();
            SBTRPanel.Visible = true;
            SBTRResults = results;

            // If there are no results. Clear the panel and display error message
            if (SBTRResults.Count == 0)
            {
                SBTRFlowerNote.Visible = false;
                SBTRBotFam.Visible = false;
                SBTREngName.Visible = false;
                SBTRError.Visible = true;
                SBTRFlowerNote.Visible = false;
                SBTRLatinName.Visible = false;
                SBTRNextPic.Visible = false;
                SBTRNoteLabel.Visible = false;
                SBTRPicBox.Visible = false;
                SBTRPicNote.Visible = false;
                SBTRPrevPic.Visible = false;
                editFlowerBttn.Visible = false;
                SBTRPrintBttn.Visible = false;
                SBTRSaveAsPDFBttn.Visible = false;
                SBTRPrevNote.Visible = false;
                SBTRNextNote.Visible = false;
                SBTRNextResult.Visible = false;
                SBTRPrevResult.Visible = false;
                viewHistogramBttn.Visible = false;
            }
            else // Display results
            {
                SBTRFlowerNote.Visible = true;
                SBTRBotFam.Visible = true;
                SBTREngName.Visible = true;
                SBTRError.Visible = false;
                SBTRFlowerNote.Visible = true;
                SBTRLatinName.Visible = true;
                SBTRNextPic.Visible = true;
                SBTRNoteLabel.Visible = true;
                SBTRPicBox.Visible = true;
                SBTRPicNote.Visible = true;
                SBTRPrevPic.Visible = true;
                editFlowerBttn.Visible = true;
                SBTRPrintBttn.Visible = true;
                SBTRSaveAsPDFBttn.Visible = true;
                SBTRPrevNote.Visible = true;
                SBTRNextNote.Visible = true;
                SBTRNextResult.Visible = true;
                SBTRPrevResult.Visible = true;
                viewHistogramBttn.Visible = true;
                SBTRNumFlowers = SBTRResults.Count;
                // Load the first flower in the list
                UpdateCurrentFlower(0);
            }

        }

        // Displays next picutre in flower object if avalible. Carousels to front if at end
        private void SBTRNextPic_Click(object sender, EventArgs e)
        {
            // If there are no pictures
            if (SBTRNumPics == 0)
            {
                // do nothing
            }
            // If at the last picture in the list
            else if (SBTRCurrentPicture + 1 >= SBTRNumPics)
            {
                // Display first image in list
                SBTRCurrentPicture = 0;
                SBTRImgs = SBTRResults[SBTRCurrentFlower].GetImages();
                SBTRPicBox.ImageLocation = SBTRImgs[SBTRCurrentPicture].GetImageLocation();
                SBTRPicNote.Text = SBTRImgs[SBTRCurrentPicture].GetNote().GetInfo();
                selectedHistogram = SBTRImgs[SBTRCurrentPicture].GetHistogram(); 
            }
            else
            {
                // Display next image in list
                SBTRImgs = SBTRResults[SBTRCurrentFlower].GetImages();
                SBTRPicBox.ImageLocation = SBTRImgs[++SBTRCurrentPicture].GetImageLocation();
                SBTRPicNote.Text = SBTRImgs[SBTRCurrentPicture].GetNote().GetInfo();
            }
        }

        // Displays previous picutre in flower object if avalible. Carousels to end if at the beginning
        private void SBTRPrevPic_Click(object sender, EventArgs e)
        {
            // If there are no pictures
            if (SBTRNumPics == 0)
            {
                // do nothing
            }
            // If at the first picture in the list
            else if (SBTRCurrentPicture - 1 < 0)
            {
                // Display last image in the list
                SBTRCurrentPicture = SBTRNumPics - 1;
                SBTRImgs = SBTRResults[SBTRCurrentFlower].GetImages();
                SBTRPicBox.ImageLocation = SBTRImgs[SBTRCurrentPicture].GetImageLocation();
                SBTRPicNote.Text = SBTRImgs[SBTRCurrentPicture].GetNote().GetInfo();
                selectedHistogram = SBTRImgs[SBTRCurrentPicture].GetHistogram();
            }
            else
            {
                // Display last image in the list
                SBTRImgs = SBTRResults[SBTRCurrentFlower].GetImages();
                SBTRPicBox.ImageLocation = SBTRImgs[--SBTRCurrentPicture].GetImageLocation();
                SBTRPicNote.Text = SBTRImgs[SBTRCurrentPicture].GetNote().GetInfo();
                selectedHistogram = SBTRImgs[SBTRCurrentPicture].GetHistogram();
            }
        }

        // Displays previous note if avalible. Carousels to end if the at the beginning of the list
        private void SBTRPrevNote_Click(object sender, EventArgs e)
        {
            // If there are no notes
            if (SBTRNumNotes == 0)
            {
                // do nothing
            }
            // If at the beginning of list
            else if (SBTRCurrentNote - 1 < 0)
            {
                // Display last note in list
                SBTRCurrentNote = SBTRNumNotes - 1;
                SBTRNotes = SBTRResults[SBTRCurrentFlower].GetNotes();
                SBTRFlowerNote.Text = SBTRNotes[SBTRCurrentNote].GetInfo();
            }
            else
            {
                // Display previous note
                SBTRNotes = SBTRResults[SBTRCurrentFlower].GetNotes();
                SBTRFlowerNote.Text = SBTRNotes[--SBTRCurrentNote].GetInfo();
            }
        }

        // Displays next note if availible. Carousels to beginning if at the end of the list
        private void SBTRNextNote_Click(object sender, EventArgs e)
        {
            // If there are no notes
            if (SBTRNumNotes == 0)
            {
                // do nothing
            }
            // If at the end of the list
            else if (SBTRCurrentNote + 1 >= SBTRNumNotes)
            {
                // Display first note
                SBTRCurrentNote = 0;
                SBTRNotes = SBTRResults[SBTRCurrentFlower].GetNotes();
                SBTRFlowerNote.Text = SBTRNotes[SBTRCurrentNote].GetInfo();
            }
            else
            {
                // Display next note
                SBTRNotes = SBTRResults[SBTRCurrentFlower].GetNotes();
                SBTRFlowerNote.Text = SBTRNotes[++SBTRCurrentNote].GetInfo();
            }
        }

        // Displays previous flower object if avalible. Carousels to end if at the beginning of the list
        private void SBTRPrevResult_Click(object sender, EventArgs e)
        {
            // If there are no results
            if(SBTRNumFlowers == 0)
            {
                // do nothing
            }
            // If at the beginning of the list
            else if (SBTRCurrentFlower - 1 < 0)
            {
                // Display the last flower object
                UpdateCurrentFlower(SBTRNumFlowers - 1);
            }
            else
            {
                // Display the previous flower object
                UpdateCurrentFlower(SBTRCurrentFlower - 1);
            }
        }

        // Displays next flower object if availible. Carousels to beginning if at the end of the list
        private void SBTRNextResult_Click(object sender, EventArgs e)
        {
            // If there are no results
            if (SBTRNumFlowers == 0)
            {
                // do nothing
            }
            // If at the end of the list
            else if (SBTRCurrentFlower + 1 >= SBTRNumFlowers)
            {
                // Display first flower object
                UpdateCurrentFlower(0);
            }
            else
            {
                // Display next flower object
                UpdateCurrentFlower(SBTRCurrentFlower + 1);
            }
        }

        // Updates the GUI with a new flower's information based on the integer passed to it
        private void UpdateCurrentFlower(int i)
        {
            SBTRCurrentFlower = i;
            SBTREngName.Text = "English Name - " + SBTRResults[i].GetEnglishName();
            SBTRLatinName.Text = "Latin Name - " + SBTRResults[i].GetLatinName();
            SBTRBotFam.Text = "Botanical Family - " + SBTRResults[i].GetBotanicalFamily();

            // Load notes
            if(SBTRResults[i].GetNotes().Count > 0)
            {
                SBTRNumNotes = SBTRResults[i].GetNotes().Count;
                SBTRFlowerNote.Text = SBTRResults[i].GetNotes()[0].GetInfo();
            }
                
            // Load images
            if (SBTRResults[i].GetImages().Count != 0)
            {
                SBTRNumPics = SBTRResults[i].GetImages().Count;
                SBTRImgs = SBTRResults[i].GetImages();
                SBTRPicBox.ImageLocation = SBTRImgs[0].GetImageLocation();
                SBTRPicBox.BackgroundImageLayout = ImageLayout.Zoom;
                SBTRPicNote.Text = SBTRImgs[0].GetNote().GetInfo();
                selectedHistogram = SBTRImgs[SBTRCurrentPicture].GetHistogram();
            }
        }

        // Uses PrintController to print the results of a search
        private void SBTRPrintBttn_Click(object sender, EventArgs e)
        {
            PrintController ctrl = new PrintController();
            ctrl.PrintResults(SBTRResults);
        }

        // Uses PdfController to save the results of a search as a PDF
        private void SBTRSaveAsPDFBttn_Click(object sender, EventArgs e)
        {
            PdfController ctrl = new PdfController();
            ctrl.SavePDF(SBTRResults);
        }



        //
        //
        // SEARCH BY RGB
        //
        //

        // Listener for search by histogram button (RGB) on main menu -> Displays search by histogram panel
        private void searchHistogramBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            searchHistogramPanel.Visible = true;
        }

        // Validates user input and uses SearchHistogramController to search database for matches
        private void searchRgbBttn_Click(object sender, EventArgs e)
        {
            int r, g, b;

            // Validates proper datatypes and range
            if (Int32.TryParse(rValueTextBox.Text, out r) 
                && Int32.TryParse(gValueTextBox.Text, out g) 
                && Int32.TryParse(bValueTextBox.Text, out b)
                && SearchHistogramController.ValidateRgb(r,g,b))
            {
                // Get matching flowers from Search Histogram controller and display to user
                DisplayResults(SearchHistogramController.SearchByHistogram(r, g, b));
            }
            else
            {
                // err
                MessageBox.Show("RGB values must be integers between 0-255");
            }
        }



        //
        //
        // ADD USER
        //
        //  

        // Listener for add user button on main menu -> Displays add user panel
        private void addUserBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            ClearAddUserFields();
            addUserPanel.Show();
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Clears fields on add user page
        public void ClearAddUserFields()
        {
            addUserNameEntry.Text = "";
            addUserUsrNameEntry.Text = "";
            addUserPasswordEntry.Text = "";
            batchImportResult.Items.Clear();
        }

        // Remove error message if field is changed
        private void addUserNameEntry_TextChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Remove error message if field is changed
        private void addUserUsrNameEntry_TextChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Remove error message if field is changed
        private void addUserPasswordEntry_TextChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Remove error message if field is changed
        private void addUserRadio1_CheckedChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Remove error message if field is changed
        private void addUserRadio2_CheckedChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // Remove error message if field is changed
        private void addUserRadio3_CheckedChanged(object sender, EventArgs e)
        {
            addUserSuccess.Hide();
            addUserError.Hide();
        }

        // On submit click, call AddUserController to save user with current values
        private void addUserSubmit_Click(object sender, EventArgs e)
        {
            // Take in user info
            String name = addUserNameEntry.Text;
            String user = addUserUsrNameEntry.Text;
            String password = addUserPasswordEntry.Text;
            int type = 1;
            if (addUserRadio1.Checked)
            {
                type = 1; // Student
            }
            if (addUserRadio2.Checked)
            {
                type = 2; // Researcher
            }
            if (addUserRadio3.Checked)
            {
                type = 3; // Admin
            }

            // Save user
            bool success = AddUserController.CreateNewUser(name, user, password, type);

            // Display results of save to user
            if (success) {
                addUserSuccess.Show();
                ClearAddUserFields();
            }
            else { addUserError.Show(); }
        }

        // On click, import batch of users from CSV file
        private void batchImportButton_Click(object sender, EventArgs e)
        {
            List<String> success = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog();

            // Only allow admin to select one csv file
            dialog.Filter = "csv files(*.csv)|*.csv";
            dialog.Multiselect = false;

            // Shows the window popup for user to choose .csv file
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Take file from admin and pass it to AddUserController
                foreach (String file in dialog.FileNames)
                {
                    try
                    {
                        // Import users with AddUserController
                        success = AddUserController.BatchImport(file);
                        // If users successfully added
                        if (success.Count > 0)
                        {
                            foreach (String user in success)
                            {
                                // Display added users to admin
                                batchImportResult.Items.Add(user);
                            }
                        }
                        else { addUserError.Show(); }
                    }
                    catch (Exception ex)
                    {
                       MessageBox.Show(ex.ToString());
                    }
                }
            }

        }

        // Displays file formatting help to user
        private void BatchImportInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Files used for import should be in the format:\n\nname,username,password,type\n\nAcceptable types are: \n1 - Standard user \n2 - Researcher \n3 - Admin", "Batch Import Info", MessageBoxButtons.OK);
        }



        //
        //
        // ADD FLOWER
        //
        //

        // Listener for add flower button on main menu -> displays add flower panel
        private void addFlowerBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            ClearAddFlowerFields();
            addFlowerPanel.Show();
            addFlowerSuccess.Hide();
            addFlowerError.Hide();
        }

        // Clears fields on add flower page 
        public void ClearAddFlowerFields()
        {
            addFlowerBotanicalFamilyEntry.Text = "";
            addFlowerNoteEntry.Text = "";
            addFlowerLatinNameEntry.Text = "";
            addFlowerEnglishNameEntry.Text = "";
            if (importedPhotos != null)
            {
                importedPhotos.Clear();
                fileNameListBox.DataSource = null;
                fileNameListBox.Items.Clear();
            }

        }

        // On submit click, call AddFlowerController to add flower with current values
        private void addFlowerSubmit_Click(object sender, EventArgs e)
        {
            String engName = addFlowerEnglishNameEntry.Text;
            String latName = addFlowerLatinNameEntry.Text;
            String family = addFlowerBotanicalFamilyEntry.Text;
            List<String> notes = new List<string>();
            String[] noteArray = addFlowerNoteEntry.Text.Split('|'); // Split up string into individual notes

            // Loop through strings and add to notes
            foreach (String n in noteArray)
            {
                notes.Add(n);
            }

            bool success = AddFlowerController.AddEntry(engName, latName, family, notes, this.importedPhotos);
            if(success)
            {
                // Clear the imported photos so they can be overwritten on other photo inputs
                if (importedPhotos != null)
                    this.importedPhotos.Clear();
                ClearAddFlowerFields();
                addFlowerSuccess.Show();
            }
            else { addFlowerError.Show(); }
        }

        // Import photo listener. Calls ImportPhotos() to get selection from user. Takes local photos and copies filenames
        private void addFlowerImportPhoto_Click(object sender, EventArgs e)
        {
            String fileName = "";
            List<String> fileNames = new List<string>();
            this.importedPhotos = ImportPhotos();

            addFlowerSuccess.Hide();
            addFlowerError.Hide();

            // Iterates through the imported photos and gets only their filenames
            foreach (FlowerImage flowerImage in this.importedPhotos)
            {
                fileName = Path.GetFileName(flowerImage.GetImageLocation());
                fileNames.Add(fileName);
            }

            // Displays filenames 
            fileNameListBox.DataSource = fileNames;
        }

        // Opens dialog to allower user to select images to import into DB
        private List<FlowerImage> ImportPhotos()
        {
            // Dialog set up
            List<FlowerImage> flowerImages = new List<FlowerImage>();
            OpenFileDialog dialog = new OpenFileDialog();
            String message = "Enter a note for the picure: ";
            String title = "Enter a note";
            String defaultValue = "New Picture added";
            FlowerImage flowerImage;

            // Filter types of images and allow multiple to be selected at a time
            dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png | Gif files(*.gif) |*.gif";
            dialog.Multiselect = true;

            // Shows the window popup for user to choose photos
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in dialog.FileNames)
                {
                    // Creates FlowerImages from the imported photo
                    try
                    {
                        if (addFlowerPanel.Visible)
                        {
                            // Prompts user to enter a note for the image
                            String info = Microsoft.VisualBasic.Interaction.InputBox(message + file, title, defaultValue);
                            flowerImage = new FlowerImage(file, info);
                        } else
                        {
                            flowerImage = new FlowerImage(file);
                        }

                        flowerImages.Add(flowerImage);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

            return flowerImages;
        }

        // Removes error message when field is changed
        private void addFlowerEnglishNameEntry_TextChanged(object sender, EventArgs e)
        {
            addFlowerSuccess.Hide();
            addFlowerError.Hide();
        }

        // Removes error message when field is changed
        private void addFlowerLatinNameEntry_TextChanged(object sender, EventArgs e)
        {
            addFlowerSuccess.Hide();
            addFlowerError.Hide();
        }

        // Removes error message when field is changed
        private void addFlowerBotanicalFamilyEntry_TextChanged(object sender, EventArgs e)
        {
            addFlowerSuccess.Hide();
            addFlowerError.Hide();
        }

        // Removes error message when field is changed
        private void addFlowerNoteEntry_TextChanged(object sender, EventArgs e)
        {
            addFlowerSuccess.Hide();
            addFlowerError.Hide();
        }



        //
        //
        // IMAGE SEARCH
        //
        //

        // Displays image search panel
        private void searchImageBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            importedPhotoText.Text = "";
            importedImgBox.Image = null;
            searchImagePanel.Visible = true;
        }

        // Calls ImportPhotos() to allow user to select an image to search with
        private void importImageButton_Click(object sender, EventArgs e)
        {
            this.importedPhotos = ImportPhotos();
            // Take first image if the user tries to select many
            FlowerImage firstPhoto = this.importedPhotos.First();
            importedPhotoText.Text = Path.GetFileName(firstPhoto.GetImageLocation());
            importedImgBox.Image = Image.FromFile(firstPhoto.GetImageLocation());
        }

        // Creates histogram of selected image and calls SearchHistogramController to compare it to other histograms
        private void initiateImageSearch_Click(object sender, EventArgs e)
        {
            if(this.importedPhotos != null)
            {
                // Create histogram with imported image
                Histogram importedHistogram = this.importedPhotos.First().GetHistogram();
                List<Flower> flowers = SearchHistogramController.SearchByHistogram(importedHistogram);
                DisplayResults(flowers);
            }
            else
            {
                // err
                MessageBox.Show("Must import image to search!");
            }
        }

        // Displays the color graph (histogram) to the user
        private void viewHistogramBttn_Click(object sender, EventArgs e)
        {
            if (SBTRNumPics > 0)
            {
                ClearAll();
                viewHistogramPanel.Visible = true;

                // Adds data points to each chart
                ViewHistogramController.AddDataPointsToChart(selectedHistogram, ref colorHistogram);
            }
        }

        // Go back to search results after viewing histogram
        private void viewHistogramBack_Click(object sender, EventArgs e)
        {
            ClearAll();
            SBTRPanel.Visible = true;
        }



        //
        //
        // EDIT FLOWER
        //
        //

        // Listener for edit flower button on the Search By Text panel-> Displays edit flower panel and loads flower object
        private void editFlowerBttn_Click(object sender, EventArgs e)
        {
            // Setup panel
            Flower editFlower = SBTRResults[SBTRCurrentFlower];
            ClearAll();
            EditFlowerSuccess.Hide();
            EditFlowerError.Hide();
            editFlowerPanel.Show();
            editFlowerEngNameEntry.Text = editFlower.GetEnglishName();
            editFlowerLatNameEntry.Text = editFlower.GetLatinName();
            editFlowerFamilyEntry.Text = editFlower.GetBotanicalFamily();
            editFlowerImages = editFlower.GetImages();
            // Setup image box
            if (SBTRNumPics > 0)
            {
                EditFlowerPicBox.ImageLocation = editFlowerImages[editFlowerPic].GetImageLocation();
                EditFlowerImageNoteEntry.Text = editFlowerImages[editFlowerPic].GetNote().GetInfo();
            }
            String noteString = "";
            // Load notes
            for (int i = 0; i < editFlower.GetNotes().Count(); i++)
            {
                noteString += "|" + editFlower.GetNote(i).GetInfo();
            }
            editFlowerNoteEntry.Text = noteString;
        }

        // Clears fields on Edit flower panel
        public void ClearEditFlowerFields()
        {
            editFlowerEngNameEntry.Text = "";
            editFlowerLatNameEntry.Text = "";
            editFlowerFamilyEntry.Text = "";
            editFlowerNoteEntry.Text = "";
            if (importedPhotos != null)
                importedPhotos.Clear();
        }

        // Displays previous flower image to the user for editing
        private void EditFlowerPrev_Click(object sender, EventArgs e)
        {
            // If not at index 0
            if(editFlowerPic > 0)
            {
                // Go to previous image and update note
                editFlowerPic--;
                EditFlowerPicBox.ImageLocation = editFlowerImages[editFlowerPic].GetImageLocation();
                EditFlowerImageNoteEntry.Text = editFlowerImages[editFlowerPic].GetNote().GetInfo();
            }
        }

        // Displays next flower image to the user for editing
        private void EditFlowerNext_Click(object sender, EventArgs e)
        {
            // If not at last index
            if (editFlowerPic < editFlowerImages.Count() - 1)
            {
                // Go to next image and update note
                editFlowerPic++;
                EditFlowerPicBox.ImageLocation = editFlowerImages[editFlowerPic].GetImageLocation();
                EditFlowerImageNoteEntry.Text = editFlowerImages[editFlowerPic].GetNote().GetInfo();
            }
        }

        // Allows user to delete an image from a flower object
        private void EditFlowerImageDelete_Click(object sender, EventArgs e)
        {
            if (editFlowerImages.Count() > 0)
            {
                // message box to confirm deletion
                var confirmResult = MessageBox.Show("Are you sure you want to delete this image?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Delete image
                    editFlowerImages.RemoveAt(editFlowerPic);
                    editFlowerPic = 0;
                    // If the flower has no more pictures after deletion
                    if (editFlowerImages.Count() == 0)
                    {
                        // Display error image in picture box
                        EditFlowerPicBox.Image = EditFlowerPicBox.ErrorImage;
                    }
                    else
                    {
                        // Display first image in flower image list
                        EditFlowerPicBox.ImageLocation = editFlowerImages[editFlowerPic].GetImageLocation();
                    }
                }
            }
            else
                MessageBox.Show("No images to delete!");

        }

        // Uses ImportPhotos() to allow user to add a new image to a flower object
        private void EditFlowerNewImage_Click(object sender, EventArgs e)
        {
            List<FlowerImage> newImages = ImportPhotos();

            // Loop through new images, add them to the list, and give them a preset note (that is editable)
            for(int i = 0; i < newImages.Count(); i++)
            {
                editFlowerImages.Add(newImages[i]);
                editFlowerImages[editFlowerImages.Count() - 1].SetNote(new Note("New picture added"));
            }

            // Clear new images list for future use
            newImages.Clear();
        }

        // Takes text from user and updates an Image's note
        private void EditFlowerImageNoteSet_Click(object sender, EventArgs e)
        {
            if (editFlowerImages.Count > 0)
                editFlowerImages[editFlowerPic].SetNote(new Note(EditFlowerImageNoteEntry.Text));
            else
                MessageBox.Show("No image. No note to edit!");
        }

        // Submit button. Uses EditFlowerController to save changes to the flower
        private void editFlowerSubmit_Click(object sender, EventArgs e)
        {
            // Take in all fields 
            Flower editFlower = SBTRResults[SBTRCurrentFlower];
            String engName = editFlowerEngNameEntry.Text;
            String latName = editFlowerLatNameEntry.Text;
            String family = editFlowerFamilyEntry.Text;
            List<String> notes = new List<string>();
            String[] noteArray = editFlowerNoteEntry.Text.Split('|');

            // Loop through noteArray and add to the flower's list of notes
            foreach (String n in noteArray)
            {
                notes.Add(n);
            }

            // Save changes using EditFlowerController
            bool success = EditFlowerController.EditEntry(engName, latName, family, notes, editFlowerImages, editFlower);

            // If successful, display message to user
            if(success)
            {
                EditFlowerSuccess.Show();
                ClearEditFlowerFields();
            }
            else { EditFlowerError.Show(); } // Else display error
        }

        // Cancels any changes made to flower object and returns to home screen
        private void editFlowerCancel_Click(object sender, EventArgs e)
        {
            ClearEditFlowerFields();
            ClearAll();
            SBTRPanel.Visible = true;
        }

        // Uses EditFlowerController to delete the selected flower. Prompts user to make sure they actually want to delete it
        private void editFlowerDelete_Click(object sender, EventArgs e)
        {
            // Promp user to confirm deletion
            var confirmResult = MessageBox.Show("Are you sure you want to delete this flower?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', delete the flower
                Flower deleteFlower = SBTRResults[SBTRCurrentFlower];
                // Delete flower using EditFlowerController
                bool success = EditFlowerController.DeleteEntry(deleteFlower);
                // If successful, display message to user
                if (success)
                {
                    EditFlowerSuccess.Show();
                    ClearEditFlowerFields();
                }
                else { EditFlowerError.Show(); } // Else display error
            }
        }



        //
        //
        // EDIT USER
        //
        //

        // Listener for edit user button on main menu -> Displays edit user panel
        private void editUserBttn_Click(object sender, EventArgs e)
        {
            ClearAll();
            EditUserPanel.Show();
            // Hide success and error messages if they are still visible
            EditUserSuccess.Hide();
            EditUserError.Hide();
            // Update with new list of users
            UpdateEditUserListBox();
        }

        // Clears all fields on Edit User panel
        public void ClearEditUserFields()
        {
            EditUserNameEntry.Text = "";
            EditUserUsrNameEntry.Text = "";
            EditUserPwdEntry.Text = "";
        }

        // Update the Listbox on Edit User panel
        public void UpdateEditUserListBox()
        {
            // Fetch list of all current users
            List<User> users = EditUserController.GetUsers();
            // Update the list box
            EditUserListBox.DataSource = users;
        }

        // Displays selected user info (and updates info when admin selects new user)
        private void EditUserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Selected user
            User selected = (User)EditUserListBox.SelectedItem;

            // Update GUI with new info
            EditUserNameEntry.Text = selected.GetName();
            EditUserUsrNameEntry.Text = selected.GetUser();
            EditUserPwdEntry.Text = "";
            if(selected.GetUserType() == 1)
            {
                EditUserRadio1.Checked = true;
            }
            if (selected.GetUserType() == 2)
            {
                EditUserRadio2.Checked = true;
            }
            if (selected.GetUserType() == 3)
            {
                EditUserRadio3.Checked = true;
            }
        }

        // Submit button. Uses EditUserController to save changes to the selected user
        private void EditUserSubmit_Click(object sender, EventArgs e)
        {
            if (EditUserListBox.SelectedItem != null)
            {   // User to be edited
                User editMe = (User)EditUserListBox.SelectedItem;
                // Take in omfp from fields
                String name = EditUserNameEntry.Text;
                String user = EditUserUsrNameEntry.Text;
                String password = EditUserPwdEntry.Text;
                
                int type = 1; // Type set to 1 (general user) if radio button not checked
                if (EditUserRadio1.Checked) // general user (view only)
                {
                    type = 1;
                }
                if (EditUserRadio2.Checked) // researcher (edit flowers)
                {
                    type = 2;
                }
                if (EditUserRadio3.Checked) // admin (full access)
                {
                    type = 3;
                }

                // Update user with EditUserController
                bool success = EditUserController.EditUser(name, user, password, type, editMe);
                // If successful, display message to user and clear fields
                if (success)
                {
                    EditUserSuccess.Show();
                    ClearEditUserFields();
                    UpdateEditUserListBox();
                }
                else { EditUserError.Show(); } // Else display error
            }
        }

        // Uses EditUserController to delete the selected user. Prompts admin to make sure they actually want to delete it
        private void EditUserDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete this User?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', delete the User using EditUserController
                User deleteMe = (User) EditUserListBox.SelectedItem;
                bool success = EditUserController.DeleteUser(deleteMe);
                // If Successful, display message to user
                if (success)
                {
                    EditUserSuccess.Show();
                    ClearEditUserFields();
                    UpdateEditUserListBox();
                }
                else { EditUserError.Show(); } // Else display error
            }
        }
    }
}