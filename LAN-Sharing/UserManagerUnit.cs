using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Media;
using System.IO;

namespace LANSharing
{
    class UserManagerUnit
    {
        private User admin= new User(); // current user used to access to this app
        private MetroFramework.Controls.MetroTile metroButton; // single istance of a button
        public Dictionary<string,User> Online_Users; // online users on LAN
        private List<MetroFramework.Controls.MetroTile> Online_Buttons = new List<MetroFramework.Controls.MetroTile>(); // list of buttons
        private Dictionary<string, User> Target_Users = new Dictionary<string, User>(); // FTP target users
        private List<MetroFramework.Controls.MetroTile> Target_Buttons = new List<MetroFramework.Controls.MetroTile>();
        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
        
        public UserManagerUnit()
        {
            try
            {
                // retrieve admin info and check LAN connection
                Online_Users = new Dictionary<string, User>();
                string name = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName; 
                string[] adminInfo = name.Split(' '); // firstName and LastName
                string localIP = GetLocalIP();
                admin = new User(adminInfo[0], adminInfo[1], "online", localIP, "3000", LANSharingApp.user_image); //set admin

                Console.WriteLine("admin info:" + adminInfo[0]+" " + adminInfo[1] + " " + localIP);
            }
            catch(Exception e) {

                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.closing = true;
                MessageBox.Show("Error: no lan connection detected, connect to a lan network then restart application.", "Error Detection Module", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }       
        }

        // method used to remove old users from the list and from the GUI
        internal void RemoveOldUsers()
        {
            try
            { 
                Dictionary<string, User>.ValueCollection All_Users = Online_Users.Values;

                if (All_Users != null && All_Users.Count > 0)
                    {
                        // use ToList in order to avoid the exception generated if we remove a person with the pointer to next person
                        foreach (User user in All_Users.ToList<User>()) // foreach user
                        {
                            var isNew = user.isNew(); // True -> no button
                            var isOld = user.isOld(); // True -> button not online

                            if (isOld) // remove
                            {
                                LANSharingApp.gui.flowLayoutPanel.Controls.Remove(user.getButton());
                                Online_Buttons.Remove(user.getButton());
                                Console.WriteLine("Rimosso: " + user.getFirstName() + " " + user.getLastName());
                                Online_Users.Remove(user.getLastName() + user.getFirstName());
                           
                            }

                        }
                    }
                      
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
        }

        // method used to add new users on the list and on the GUI
        public void AddNewUsers()
        {   
                try
                {
          
                   Dictionary<string, User>.ValueCollection All_Users = Online_Users.Values;
                    if (All_Users != null && All_Users.Count > 0)
                    {
                        foreach (User user in All_Users)
                        {
                            if (user.isNew() && user.isOnline())
                            {
                            //sound
                            
                            // create button  for user on the form
                            metroButton = new MetroFramework.Controls.MetroTile();
                              
                                metroButton.Name = user.getFirstName() + "," + user.getLastName() + "," + user.getIp() + "," + user.getPort();
                                metroButton.Text = user.getFirstName() + "\n" + user.getLastName();
                                metroButton.Style = MetroFramework.MetroColorStyle.Silver;// online but not selected

                            //metroButton.TileImage = Image.FromFile("C:\\ProgramData\\Microsoft\\User Account Pictures\\user-32.png");
                                metroButton.TileImage = user.getImage();
                                
                                metroButton.UseTileImage = true;
                                metroButton.Size = new Size(70, 70);
                                metroButton.TileImageAlign = ContentAlignment.TopCenter;
                                metroButton.Click += new EventHandler(TargetUserButton_Click);

                                Online_Buttons.Add(metroButton);
                                user.addButton(metroButton);
                                LANSharingApp.gui.flowLayoutPanel.Controls.Add(metroButton);
                           

                                user.unsetNew();// user is not old but is not new now
                            }
                        }
                    }
                
                }catch(Exception e) {
                    Console.WriteLine(e.ToString());
                    LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                    LANSharingApp.serverThread.Join();
                    Application.Exit();
            }

           
        }

        internal void UpdateImageButtons()
        {
            try
            {
                Dictionary<string, User>.ValueCollection All_Users = Online_Users.Values;
                if (All_Users != null && All_Users.Count > 0)
                {
                    foreach (User user in All_Users.ToList<User>())
                    {
                        Bitmap b1 = new Bitmap(user.getImage());
                        Bitmap b2 = new Bitmap(user.getButton().TileImage);

                        if (!ImageCompareString(b1,b2))
                        {
                            //get button
                            metroButton = user.getButton();
                            //remove obsolete button with old image
                            LANSharingApp.gui.flowLayoutPanel.Controls.Remove(user.getButton());
                            //add new button with new image
                            metroButton.TileImage= user.getImage();
                            LANSharingApp.gui.flowLayoutPanel.Controls.Add(metroButton);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
           

        }

        // return a list of all users online
        public Dictionary<String, User> getOnlineUsers()
        {
            return Online_Users;
        }

        public string getAdminState()
        {
            return this.admin.getState();
        }

        //check user presence on list
        internal bool isPresent(string v)
        {
            return Online_Users.ContainsKey(v); 
        }

        //reset the user timer for the online presence
        internal void resetUserTimer(string infoUser)
        {
            User user;
            Online_Users.TryGetValue(infoUser, out user);
            user.resetUser();
        }

        //make them Silver again :)
        public void clearMetroButtons()
        {
           foreach(MetroFramework.Controls.MetroTile m in Target_Buttons.ToList<MetroFramework.Controls.MetroTile>())
           {
                m.Style = MetroFramework.MetroColorStyle.Silver;
                //not a target
                Target_Buttons.Remove(m);
            }
        }
        

        // method called when the metroButton is clicked
        private void TargetUserButton_Click(object sender, EventArgs e)
        {
            // method to add or remove users on the selected for FTP protocol
            MetroFramework.Controls.MetroTile changeStateButton = sender as MetroFramework.Controls.MetroTile;

            if (changeStateButton.Style == MetroFramework.MetroColorStyle.Silver)
            {
                //FTP target
                Target_Buttons.Add(changeStateButton);
                changeStateButton.Style = MetroFramework.MetroColorStyle.Blue;
            }
            else
            {
                //not a target
                Target_Buttons.Remove(changeStateButton);
                changeStateButton.Style = MetroFramework.MetroColorStyle.Silver;
            }
        }

        // method called when the SendButton is clicked
        public void SendButtonClick()
        {
            try
            {


                if (Target_Buttons.Count > 0) // if targets are  > 0
                {
                    foreach (MetroFramework.Controls.MetroTile metroButton in Target_Buttons)
                    {// each thread will start with an argument = name of user
                        Thread clientThread =
                            new Thread(() => LANSharingApp.client.EntryPoint(metroButton.Name))
                            { Name = "clientThread" };
                        clientThread.IsBackground = true;
                        clientThread.Start();

                    }

                }
                else
                {
                    MessageBox.Show("Please select an user");
                    if (LANSharingApp.sysSoundFlag == 1)
                    {
                        audio_error.Play();
                    }
                }
                clearMetroButtons();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }
        }

        //add user to the list, or update an existing one
        public void addUser(User user)
        {

            if (!Online_Users.ContainsKey(user.getLastName() + user.getFirstName()))
            {
                Console.WriteLine("Aggiunto in lista :" + user.getFirstName() + " " + user.getLastName() + " " + user.getIp() + " " + user.getPort() + " " + user.getImage().ToString());
                // attention: each user is set as new from the begin
                Online_Users.Add(user.getLastName() + user.getFirstName(), user);
            }
            else {//already in list
                
                //update last udp image
                    Online_Users[user.getLastName() + user.getFirstName()].setImage(user.getImage());            
              }
           
        }

        public User getAdmin()
        {
            return admin;
        }

        public void changeAdminState(string s)
        {         
            this.admin.setState(s);
        }

        public string GetLocalIP()
        {

         
            string localIP = null; ;

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }

                //check if no connection
                if (localIP == null || !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception("No LAN connection detected");

            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }

            return localIP;
           
  
            /*
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            // Get the IP  
            string myIP = null;
            int l = Dns.GetHostAddresses(Dns.GetHostName()).Length;

            if(l>0)
                myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            if (myIP == null || !System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("No LAN connection detected");

            return myIP;
    */
        }

        public static bool ImageCompareString(Bitmap firstImage, Bitmap secondImage)
        {

            MemoryStream ms = new MemoryStream();
            firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String firstBitmap = Convert.ToBase64String(ms.ToArray());

             ms.Position = 0;
            secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String secondBitmap = Convert.ToBase64String(ms.ToArray());

            if (firstBitmap.Equals(secondBitmap))
            {
                return true;
            }
            else
            {
                return false;
             }

        }
    }
}