using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Media;
using System.IO.MemoryMappedFiles;

namespace LANSharing
{
    public partial class GUI : MetroFramework.Forms.MetroForm
    {

        public static string myPath;
        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name

        //file used to IPC 
        public static MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("myMappedFile");

        public GUI(string selectedPath)
        {
            InitializeComponent();

            try
            {
                //update user_image
                this.user_tile.TileImage = LANSharingApp.user_image;
                this.user_tile.Refresh();

                // link menu to the task bar
                this.taskbarIcon.ContextMenuStrip = contextMenuTaskBar;

                // visualize admin info
                firstNameLabel.Text = LANSharingApp.umu.getAdmin().getFirstName();
                lastNameLabel.Text = LANSharingApp.umu.getAdmin().getLastName();

                lock (LANSharingApp.lockerPathSend)
                {
                    //show path
                    if (LANSharingApp.pathSend != null)
                    {
                    
                        pathBox.Text = LANSharingApp.pathSend;
                    }                
                }
                    
                
                //manage two possible state: Online/Offline
                if (LANSharingApp.umu.getAdmin().getState() == "online")
                {
                    stateLabel.Text = "ONLINE";
                    offlineMenuItem.Checked = false;
                    onlineMenuItem.Checked = true;
                    state_offline_dot.Visible = false;
                    state_online_dot.Visible = true;
                }
                else
                {
                    stateLabel.Text = "OFFLINE";
                    offlineMenuItem.Checked = true;
                    onlineMenuItem.Checked = false;
                    state_offline_dot.Visible = true;
                    state_online_dot.Visible = false;
                }

       

                // set timer for refresh button, in order to update the lsit of Online Users
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = (1000);
                timer.Tick += new EventHandler(Timer_Tick_Tock);
                timer.Start();
        
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
            }
            

        }

        private void GUI_Load(object sender, EventArgs e)
        {
         
            user_tile.TileImage = LANSharingApp.user_image;
            user_tile.Refresh();

            //default: Online on launch

            // set StateButton
            if (LANSharingApp.umu.getAdminState().CompareTo("online") == 0)
            {
                state_offline_dot.Visible = false;
                state_online_dot.Visible = true;
            }
            else //offline
            {
                state_offline_dot.Visible = true;
                state_online_dot.Visible = false;
            }

            lock (LANSharingApp.lockerPathSend)
            {
                //show path
                if (LANSharingApp.pathSend != null)
                {              
                    pathBox.Text = LANSharingApp.pathSend;
                }
            }      
           
           

            // set refresh button background color
            refreshButton.Style = MetroFramework.MetroColorStyle.White;
        }

        // use: 1) autoRefresh called by timer, without calling the buttonRefresh 2)called from the buttonRefresh
        private void Timer_Tick_Tock(object sender, EventArgs e)
        {
            
            try
            {
                //call the 2 methods thar update the list of Online Users
                LANSharingApp.umu.RemoveOldUsers();
                LANSharingApp.umu.AddNewUsers();
                LANSharingApp.umu.UpdateImageButtons();

                //LAUNCH GUI WITH NEW PATH SEND IF THE USER INTERACT WITH NEW FILES/DIRECTORIES WITH RIGTH CLICK ON THEM
                //....

                // read the integer value at position 500
                MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
                int l = accessor.ReadInt32(0);
                accessor.Flush();
                // print it to the console
               
                if (l != 0)
                {
                    //get path as bytes
                    byte[] Buffer = new byte[l];
                    accessor.ReadArray(4, Buffer, 0, Buffer.Length);
                    accessor.Flush();
                    //convert bytes to string
                    string newPath = ASCIIEncoding.ASCII.GetString(Buffer);
                    // Console.WriteLine("The newPath is " + newPath);

                   
                    lock (LANSharingApp.lockerPathSend)
                    {
                        LANSharingApp.pathSend = newPath;
                    }
  
                        this.pathBox.Text = newPath;
                        LANSharingApp.umu.clearMetroButtons();
                        base.SetVisibleCore(true);
                        this.WindowState = FormWindowState.Normal;

                    //invalidate path as old one
                    accessor.Write(0,0);
                    accessor.Flush();
            
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
            }

        }

        // event: shareButtonClick
        private void shareButton_Click(object sender, EventArgs e)
        {
                 //if path exist, call the procedure FTP
                if(Directory.Exists(pathBox.Text) || File.Exists(pathBox.Text))
                {
                    lock (LANSharingApp.lockerPathSend)
                    {
                        LANSharingApp.pathSend = pathBox.Text;
                    }

                        LANSharingApp.umu.SendButtonClick();
                }
                else //if path doesn't exist, advide admin
                {
                    if (LANSharingApp.sysSoundFlag == 1)
                    {
                        audio_error.Play();
                    }
                    MessageBox.Show("Selected file or folder doesn't exist!");
                }
        }

        //cancel operation
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.pathBox.Text = "";
            LANSharingApp.umu.clearMetroButtons();
            //base.SetVisibleCore(false);
            //this.WindowState = FormWindowState.Minimized;
        }

        private void changeAdminState_Click(object sender, EventArgs e)
        {
            //switch online/offline 
            MetroFramework.Controls.MetroTile changeState = sender as MetroFramework.Controls.MetroTile;

            if (LANSharingApp.umu.getAdminState().Equals("online"))
            {
                LANSharingApp.umu.changeAdminState("offline");
                LANSharingApp.mre.Reset(); //block server with ManualResetEvent
                stateLabel.Text = "ONLINE";
                offlineMenuItem.Checked = true;
                onlineMenuItem.Checked = false;
                state_offline_dot.Visible = true;
                state_online_dot.Visible = false;

            }
            else
            {
                LANSharingApp.umu.changeAdminState("online");
                LANSharingApp.mre.Set(); //unblock server with ManualResetEvent
                stateLabel.Text = "OFFLINE";
                offlineMenuItem.Checked = false;
                onlineMenuItem.Checked = true;
                state_offline_dot.Visible = false;
                state_online_dot.Visible = true;
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            //call the method to update the list of Online Users
            this.Timer_Tick_Tock(sender, e);
            MessageFormError mfe = new MessageFormError("Online users list has been refreshed");
            mfe.ShowDialog();
        }

        private void settingsButton_Click(object sender, MouseEventArgs e)
        {
            // open settings
            Settings s = new Settings();
            s.StartPosition = FormStartPosition.CenterScreen;
            s.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            s.Show();
        }

       
        private void taskbarIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //double clcik -> open GUI
            base.SetVisibleCore(true);
            this.WindowState = FormWindowState.Normal;
           
        }

   
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (LANSharingApp.sysSoundFlag == 1)
            {
                audio_error.Play();
            }
            //open message on exit
            switch (MessageBox.Show(this, "Do you wanna quit the application?", "Exit", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    break;
                default:
                    FormClosingEventArgs fcea = new FormClosingEventArgs(CloseReason.WindowsShutDown, false);
                    LANSharingApp.closing = true;
                    LANSharingApp.serverThread.Join();
                    base.OnFormClosing(fcea);
                    this.Dispose(); //release resources non auto-realised

                    LANSharingApp.flushTmpDirectory();
                    Application.Exit();
                    break;
            }
        }

        private void openIconContextMenu_Click(object sender, EventArgs e)
        {
            //open GUI
            base.SetVisibleCore(true);
            this.WindowState = FormWindowState.Normal;
        }

        
        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            // open settings from task bar
            Settings s = new Settings();
            s.StartPosition = FormStartPosition.CenterScreen;
            s.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            s.Show();
        }

        
        private void offlineIconContextMenu_Click(object sender, EventArgs e)
        {
            // change the state offline from the task bar

            if (LANSharingApp.umu.getAdmin().isOnline()) 
            {
                LANSharingApp.umu.changeAdminState("offline");
                LANSharingApp.mre.Reset(); // block server with ManualResetEvent
                stateLabel.Text = "OFFLINE";
                offlineMenuItem.Checked = true;
                onlineMenuItem.Checked = false;
                state_offline_dot.Visible = true;
                state_online_dot.Visible = false;

            }
                
        }

        private void onlineIconContextMenu_Click(object sender, EventArgs e)
        {
            // change the state online from the task bar
            try
            {
                if (!LANSharingApp.umu.getAdmin().isOnline()) 
                {
                    LANSharingApp.umu.changeAdminState("online");
                    LANSharingApp.mre.Set(); // unblock server with ManualResetEvent
                    stateLabel.Text = "ONLINE";
                    offlineMenuItem.Checked = false;
                    onlineMenuItem.Checked = true;
                    state_offline_dot.Visible = false;
                    state_online_dot.Visible = true;

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
            }
           
        }

        // override
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //case close app
            if (LANSharingApp.closing)
                return;
            //case shutdown pc
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            //close form, but the app is still in background
            base.SetVisibleCore(false);
            //this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
            taskbarIcon.Visible = true;
        }

        protected override void SetVisibleCore(bool value)
        {
            lock (LANSharingApp.lockerPathSend)
            {
                //hide GUI on launch
                if (LANSharingApp.pathSend == null)
                    base.SetVisibleCore(false);
                else
                    base.SetVisibleCore(true);
                //show app and path on launch from file/directory
            }

        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //release resources non free from the application
            //releases resources that are not automatically released
            this.Dispose();
            this.Close();
        }

        private void contextMenuStripTaskbarIcon_Opening(object sender, CancelEventArgs e)
        {
            //to implement only if need
        }

        private void browseDirectory_Click(object sender, EventArgs e)
        {
            // choose a directory to send

            var t = new Thread((ThreadStart)(() => {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                    return;

                 myPath= fbd.SelectedPath;
                
            }));

            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA); // application born as MTA, but i need STA for graphical purpose
            t.Start();
            t.Join();

            pathBox.Text = myPath;                // show directory
        }

        private void browseFile_Click(object sender, EventArgs e)
        {
           
            // search for a file to send
            var t = new Thread((ThreadStart)(() => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "All Files|*.*";
                ofd.Title = "Select a File";
              
                if (ofd.ShowDialog() == DialogResult.Cancel)
                    return;

             
               myPath = ofd.FileName;
               

            }));

            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA); // application born as MTA, but i need STA for graphical purpose
            t.Start();
            t.Join(); 

            pathBox.Text = myPath;  // show file
            //pathBox.Size = TextRenderer.MeasureText(myPath, pathBox.Font); TODO !!!
        }

        private void lastName_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void user_tile_Click(object sender, EventArgs e)
        {
            if (LANSharingApp.umu.getAdminState().Equals("online"))
            {
                LANSharingApp.umu.changeAdminState("offline");
                stateLabel.Text = "OFFLINE";
                offlineMenuItem.Checked = true;
                onlineMenuItem.Checked = false;
                state_offline_dot.Visible = true;
                state_online_dot.Visible = false;

            }
            else
            {
                LANSharingApp.umu.changeAdminState("online");
                stateLabel.Text = "ONLINE";
                offlineMenuItem.Checked = false;
                onlineMenuItem.Checked = true;
                state_offline_dot.Visible = false;
                state_online_dot.Visible = true;

            }
            
        }

        private void description_Click(object sender, EventArgs e)
        {

        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void state_online_dot_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void browseDirectory_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(browseDirectory, "Share a folder");
        }

        private void user_tile_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(user_tile, "Change your user state");
        }

        private void browseFile_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(browseFile, "Share a file");
        }

        private void settingsButton_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(settingsButton, "Settings");
        }

        private void refreshButton_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(refreshButton, "Refresh contacts");
        }

        private void cancelButton_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(cancelButton, "Clear all");
        }

        private void shareButton_MouseEnter(object sender, EventArgs e)
        {
            toolTipShareDirectory.ShowAlways = true;
            toolTipShareDirectory.SetToolTip(shareButton, "Send");
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {

        }

        public void refreshUser_tile(Image i) {
            user_tile.TileImage = i;
        }
    }
}
