using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MetroFramework.Controls;
using System.Security.Principal;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace LANSharing
{
    public class User
    {        
        private string firstName;
        private string lastName;
        private string state;
        private IPAddress ip;
        private int port;
        private bool imNew;
        private bool imOld;
        private System.Timers.Timer timer;
        private MetroTile listButton;
        private Image image;

        public User()
        {
            
        }

        public User(string name, string surname, string state, string ip, string port, Image i)
        {
            
            this.firstName = name;
            this.lastName = surname;
            this.state = state;
            this.ip = IPAddress.Parse(ip);
            this.port = int.Parse(port);
            this.imNew = true;
            this.imOld = false;
            this.image = i;
            //timer is used to set the new/old of a user in the list
            string[] c = new string[2];
            c[0] = name;
            c[1] = surname;
            archive_Image(i, String.Join("_", c));
            timer = new System.Timers.Timer(7000);
            timer.Elapsed +=  TimeExpired;
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        //getters and setters
        public void setImage(Image i)
        {
            this.image = i;
        }

        public Image getImage()
        {
            return this.image;
        }

        public string getFirstName()
        {
            return firstName;
        }

        public void setFirstName(string n)
        {
            this.firstName = n;
        }

        public string getLastName()
        {
            return this.lastName;
        }

        public void setLastName(string c)
        {
            this.lastName = c;
        }

        public string getState()
        {
            return this.state;
        }

        public void setState(string s)
        {
            this.state = s;
        }

        public bool isNew()
        {
            return imNew;
        }

        public bool isOld()
        {
            return imOld;
        }

        // disable new property
        public void unsetNew()
        {
            imNew = false;
        }
   
        public IPAddress getIp()
        {
            return ip;
        }

        public void setIP(string localIP)
        {
            this.ip = IPAddress.Parse(localIP);
        }

        public int getPort()
        {
            return port;
        }

        public bool isOnline()
        {
            if (state.Equals("online"))
                return true;
            else
                return false;
        }

        public string getString()
        {            
            string compressedImage = ImageToBase64(LANSharingApp.user_small_image, ImageFormat.Jpeg);

            return firstName + "," + lastName + "," + state + "," + ip.ToString() + "," + port + "," + compressedImage;        
        }

        public string ImageToBase64(Image image,System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public bool isEqual(User p)
        {
            return (p.getLastName().CompareTo(lastName) == 0)
                && (p.getFirstName().CompareTo(firstName) == 0)
                && (p.getIp().ToString().CompareTo(ip.ToString()) == 0)
                && (p.getPort() == port);
        }

        public void resetUser()
        {
            timer.Stop();
            imOld = false;
            timer.Start();
        }
      
        private void TimeExpired(object sender, System.Timers.ElapsedEventArgs e)
        {
                imOld = true;            
        }

        // add metrotile button to the user
        internal void addButton(MetroTile metroButton)
        {
            listButton = metroButton;
        }

        // ritorno il bottone associato alla persona
        internal MetroTile getButton()
        {
            return listButton;
        }

        private void archive_Image(Image image, string s)
        {
            string[] info = s.Split('_');

          
            if (!Directory.Exists(Application.StartupPath + @"\\Users"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\\Users");
            }
            string pathImage = Application.StartupPath + @"\\Users\\" + s;

            image.Save(pathImage, ImageFormat.Jpeg);
                    
        }     
    }
}
