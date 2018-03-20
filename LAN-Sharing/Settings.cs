using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;

namespace LANSharing
{
    public partial class Settings : MetroFramework.Forms.MetroForm
    {
        public static string myPath_image;

        private string title = "Settings";
        private string advise = "ATTENTION: The automatic save allow to receive any  \n type of file/directory from any source on the LAN.";
        private string lastPath = LANSharingApp.pathSave;
        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
        private int flag_default_image = LANSharingApp.image_flag; //if 0 default image is used
        private int modified_image; //if the image has changed
        Image old_image;
        Image actual_image = LANSharingApp.user_image;
        static string path_80 = LANSharingApp.user_image_path;
        static string path_32 = LANSharingApp.user_small_image_path;
        int count_change_image = 0;
        Image actual_small_image = global::LANSharing.Properties.Resources.user_32;

        public Settings()
        {
            InitializeComponent();
        }

        // action performed on settings load
        private void Settings_Load(object sender, EventArgs e)
        {
            LANSharingApp.gui.settingsButton.Enabled = false;
            //check for user image 
            if (LANSharingApp.image_flag == 1)
            {
                user_tile.TileImage = LANSharingApp.user_image;
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }

            // initialize title
            this.Text = title;

            // initialize automatic path
            lock (LANSharingApp.lockerPathSave)
            {
                destinationPath.Text = LANSharingApp.pathSave;
            }

            // initialize checkboxes automaticSave
            if (LANSharingApp.saveProfile == 0)
                radioButtonAutomaticSave.Checked = true;
            else if (LANSharingApp.saveProfile == 1)
                radioButtonNoAutomaticDefault.Checked = true;
            else
                radioButtonNoAutomaticComplete.Checked = true;

            // initialize save button
            saveButton.Enabled = false;

            // initialize textboxes
            textBoxFirstName.Text = LANSharingApp.umu.getAdmin().getFirstName();
            textBoxLastName.Text = LANSharingApp.umu.getAdmin().getLastName();

            //initialize audio 
            if (LANSharingApp.sysSoundFlag == 1)
            {
                this.soundTile.TileImage = global::LANSharing.Properties.Resources.sound_on40;
            }
            else
            {
                this.soundTile.TileImage = global::LANSharing.Properties.Resources.sound_off40;
            }

            old_image = LANSharingApp.user_image;
            actual_image = LANSharingApp.user_image;
        }

        // cancel operation
        private void Cancel_Click(object sender, EventArgs e)
        {
            lock (LANSharingApp.lockerPathSave)
            {
                LANSharingApp.pathSave = lastPath;
            }
            //LANSharingApp.gui.settingsButton.Enabled = false;
            LANSharingApp.gui.settingsButton.Enabled = true;
            LANSharingApp.gui.settingsButton.Refresh();
            this.Close();
        }

        // search for a path save
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                // need a lambda function with a new thread to perform operation
                // cause thread are MTA on launch
                Thread t = new Thread(() =>
                {
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    {
                        DialogResult dr = fbd.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            lock (LANSharingApp.lockerPathSave)
                            {
                                LANSharingApp.pathSave = fbd.SelectedPath;
                            }

                        }
                    }
                });

                t.IsBackground = true;
                // must use STA for the new thread
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();

                lock (LANSharingApp.lockerPathSave)
                {
                    // update the path
                    if (destinationPath.Text.CompareTo(LANSharingApp.pathSave) != 0)
                    {
                        destinationPath.Text = LANSharingApp.pathSave;
                        // notify modification
                        saveButton.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            LANSharingApp.gui.settingsButton.Enabled = true;
            LANSharingApp.gui.settingsButton.Refresh();
            try
            {
                count_change_image++;

                if (flag_default_image == 1)
                {
                    LANSharingApp.image_flag = 1;
                }
                else
                {
                    LANSharingApp.image_flag = 0;
                }

                LANSharingApp.user_image = actual_image;

                LANSharingApp.gui.user_tile.TileImage = actual_image;
                LANSharingApp.gui.user_tile.Refresh();
                LANSharingApp.user_image_path = path_80;
                LANSharingApp.user_small_image = actual_small_image;
                //Console.WriteLine("New path: " + LANSharingApp.user_image_path);
                LANSharingApp.user_image_path = path_80;
                LANSharingApp.user_small_image_path = path_32;

                // save radio button modification
                if (radioButtonNoAutomaticComplete.Checked)
                    LANSharingApp.saveProfile = 2;
                else if (radioButtonNoAutomaticDefault.Checked)
                    LANSharingApp.saveProfile = 1;
                else
                    LANSharingApp.saveProfile = 0;

                // save new path
                lock (LANSharingApp.lockerPathSave)
                {
                    LANSharingApp.pathSave = destinationPath.Text;
                }

                // save changes on admin info
                LANSharingApp.umu.getAdmin().setFirstName(textBoxFirstName.Text);
                LANSharingApp.umu.getAdmin().setLastName(textBoxLastName.Text);
                string fn = LANSharingApp.umu.getAdmin().getFirstName();
                string ln = LANSharingApp.umu.getAdmin().getLastName();
                string[] c = new string[2];
                c[0] = fn;
                c[1] = ln;

                LANSharingApp.UpdateUserPreference();
                // clear operation
                saveButton.Enabled = false;
                LANSharingApp.gui.settingsButton.Enabled = true;
                LANSharingApp.gui.settingsButton.Refresh();
                this.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
        }

        private void checkBox_Click(object sender, MouseEventArgs e)
        {
            // check if there is a change
            saveButton.Enabled = true;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {


                // ask if the user want really modify the form
                if (saveButton.Enabled)
                {
                    if (LANSharingApp.sysSoundFlag == 1)
                    {
                        audio_error.Play();
                    }

                    switch (MessageBox.Show(this, "Do you wanna save your changes?", "Exit from Settings", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.No:
                            lock (LANSharingApp.lockerPathSave)
                            {
                                LANSharingApp.pathSave = lastPath;
                            }
                            break;
                        default:
                            Save_Click(sender, e);
                            break;
                    }
                }
                LANSharingApp.gui.settingsButton.Enabled = true;
                LANSharingApp.gui.settingsButton.Refresh();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
        }

        // notify when name or surname of admin change
        private void textBoxFistName_TextChanged(object sender, EventArgs e)
        {
            if (!(textBoxFirstName.Text.CompareTo(LANSharingApp.umu.getAdmin().getFirstName()) == 0))
                saveButton.Enabled = true;
        }

        private void textBoxLastName_TextChanged(object sender, EventArgs e)
        {
            if (!(textBoxLastName.Text.CompareTo(LANSharingApp.umu.getAdmin().getLastName()) == 0))
                saveButton.Enabled = true;
        }

        private void radioButtonEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void user_tile_Click(object sender, EventArgs e)
        {
            try
            {


                // search for a user image 
                old_image = LANSharingApp.user_image;
                int old_flag = LANSharingApp.image_flag;
                Console.WriteLine("Old Flag: " + old_flag);

                var t_image = new Thread((ThreadStart)(() =>
                {
                    OpenFileDialog ofd_image = new OpenFileDialog();
                    ofd_image.Filter = "JPG|*.jpg;*.jpeg|BMP|*.bmp|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff";
                    ofd_image.Title = "Select a File";

                    if (ofd_image.ShowDialog() == DialogResult.Cancel)
                    {

                        return;
                    }

                    myPath_image = ofd_image.FileName;

                    Console.WriteLine("Path Immagine: " + myPath_image);
                }));
                t_image.IsBackground = true;
                t_image.SetApartmentState(ApartmentState.STA); // application born as MTA, but i need STA for graphical purpose
                t_image.Start();
                t_image.Join();
                Image image;

                if (myPath_image != null)
                {
                    path_80 = myPath_image;
                    modified_image = 1; //user image has changed, used clicked "ok"
                    flag_default_image = 1;
                    //image = Image.FromFile(@"" + myPath_image);             
                    using (var bmpTemp = new Bitmap(@"" + myPath_image))
                    {
                        if (Array.IndexOf(bmpTemp.PropertyIdList, 274) > -1)
                        {
                            var orientation = (int)bmpTemp.GetPropertyItem(274).Value[0];
                            switch (orientation)
                            {
                                case 1:
                                    // No rotation required.
                                    break;
                                case 2:
                                    bmpTemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    break;
                                case 3:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    break;
                                case 4:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate180FlipX);
                                    break;
                                case 5:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate90FlipX);
                                    break;
                                case 6:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    break;
                                case 7:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate270FlipX);
                                    break;
                                case 8:
                                    bmpTemp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    break;
                            }
                            // This EXIF data is now invalid and should be removed.
                            bmpTemp.RemovePropertyItem(274);
                        }
                        image = new Bitmap(bmpTemp);
                    }



                    image = ScaleImage(image, 80, 80);


                    button2.Visible = true;
                    saveButton.Enabled = true;
                    this.user_tile.TileImage = image;

                    old_image = actual_image;
                    actual_image = image;
                    actual_small_image = ScaleImage(image, 32, 32);
                    Console.WriteLine("Modified: " + modified_image + " Old image: " + old_image + " New Image: " + actual_image);
                    user_tile.Refresh();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }

        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

         
            saveUserImage(newImage, maxHeight);

            return newImage;
        }

        public static void saveUserImage(Image image, int dim)
        {
            if (!Directory.Exists(Application.StartupPath + @"\\Admin"))
            {              
                Directory.CreateDirectory(Application.StartupPath + @"\\Admin");
            }
        
            string pathImage = Application.StartupPath + @"\\Admin\\admin_Image_" + dim;

            if (dim == 80)
            {
                path_80 = pathImage;
            }
            else
            {
                path_32 = pathImage;
            }
           
            image.Save(pathImage, ImageFormat.Jpeg);
            
        }

        //clearbutton
        private void button2_Click(object sender, EventArgs e)
        {
            path_80 = "no";
            path_32 = "no";
            modified_image = 1;
            old_image = actual_image;
            actual_image = global::LANSharing.Properties.Resources.user_80;
            actual_small_image = global::LANSharing.Properties.Resources.user_32;

            user_tile.TileImage = global::LANSharing.Properties.Resources.user_80;
            user_tile.Refresh();
            flag_default_image = 0;
            Console.WriteLine("Modified: " + modified_image + " Old image: " + old_image + " New Image: " + actual_image);


            saveButton.Enabled = true;
            button2.Visible = false;

        }

        private void soundTile_Click(object sender, EventArgs e)
        {
            if (LANSharingApp.sysSoundFlag == 1)
            {   //sounds disabled
                Console.WriteLine("Sound disabled");
                LANSharingApp.sysSoundFlag = 0;
                soundTile.TileImage = global::LANSharing.Properties.Resources.sound_off40;
                saveButton.Enabled = true;
            }
            else
            {
                //sounds enabled
                Console.WriteLine("Sound enabled");
                LANSharingApp.sysSoundFlag = 1;
                soundTile.TileImage = global::LANSharing.Properties.Resources.sound_on40;
                saveButton.Enabled = true;
            }
            soundTile.Refresh();

        }

        private void soundTile_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(soundTile, "Click to Enable/Disable system sounds");
        }

        private string archive_Image(Image image, string s, int count_change_image)
        {
            //scale big 80*80 image to a 32*32 image
            image = ScaleImage(image, 32, 32);
            //save the image as a "firstname_lastname" file into the project folder
            //overwrite in case of same name file

            image.Save(Application.StartupPath+"@\\" + s + count_change_image, ImageFormat.Jpeg);
            // string path_IMAGE = s+ count_change_image;
            Console.WriteLine("Small image path: " + s + count_change_image);

            image.Dispose();
            return s + count_change_image;
        }
    }
}
