using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Media;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace LANSharing
{
    static class LANSharingApp
    {
        public static UserManagerUnit umu;
        public static GUI gui;
        public static Server server;
        public static Client client;
        public static Thread serverThread;
        public static string tmpPath = Application.StartupPath + @"\\tmpFiles";
        public static string pathSend = null; // path of data to send
        public static string pathSave = "C:\\Users\\" + Environment.UserName + "\\Downloads"; //default path to save incoming files and directories     
        public static bool running = false; // flag to detect if the application was launched more than one time
        public static bool closing = false; // flag used from threads to stop their task
        public static int saveProfile = 0; // 0 1 2 are 3 different profile to save
        
        public static ManualResetEvent mre = null; //tell to the server if admin is online/offline
        public static RegistryKey key;  // used to modify local machine registry and add app to right click menu

        public static int image_flag = 0; //if 0 default user image is set
        public static Image user_image= global::LANSharing.Properties.Resources.user_80; //userimage to display in the main gui 
        public static Image user_small_image = global::LANSharing.Properties.Resources.user_32;//userimage to display in the main gui 
        public static string user_image_path = "no";//path of the user_image 
        public static string user_small_image_path = "no";//path of the small image to send via udp

        public static int sysSoundFlag = 1; //audio enabled for default

        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name

        //list of temp not deleted on close or crash
        public static List<string> tempFileRecv = new List<string>();
        public static List<string> tempFileSend = new List<string>();

        //lock to modify global paths thread safe
        public static readonly object lockerPathSend = new object();
        public static readonly object lockerPathSave = new object();

        /// <summary>
        /// EntryPoint of the LAN-Sharing Application
        /// </summary>
        /// 

        static LANSharingApp() //executed before MAIN
        {
            // Codice per l'aggiunta dell'opzione al context menu di Windows FILE
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\Classes\\*\\Shell\\LAN-Sharing");
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\Classes\\*\\Shell\\LAN-Sharing\\command");

            key.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"%1\"");

            // Codice per l'aggiunta dell'opzione al context menu di Windows FOLDER
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\CLASSES\Folder\shell\LAN-Sharing");
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\CLASSES\Folder\shell\LAN-Sharing\command");

            key.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"%1\"");
        
        }

        [MTAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists(tmpPath))
                Directory.CreateDirectory(tmpPath);

            //check if the app is already running
            running = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
            //Console.WriteLine("ALready running? " + running);

            if (running)
            {

                string selected = null;

                if (args.Length == 1) //apertura da context menu
                {
                    selected = args[0];
                    lock (LANSharingApp.lockerPathSend)
                    {
                        pathSend = selected;
                    }
                   
                }
                // open the memory-mapped 
                MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("myMappedFile");
                // declare accessor to write on file
                MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
                if (selected != null)
                {
                    //write in the file: Length|Path
                    //4 offset cause length is on 32 bit
                    byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(selected);
                    accessor.Write(0, Buffer.Length);
                    accessor.Flush();
                    accessor.WriteArray(4, Buffer, 0, Buffer.Length);
                    accessor.Flush();
                    //write path in the memory mapped file
                }
                else
                {
                    //write in the file: Lenght|Path
                    accessor.Write(0, 0);
                }

                //MessageBox.Show("Application already running in background!!!");
                closing = false;

            }
            else {
                if (!closing)// there are no process active
                {
                    // create a memory-mapped file of length 1000 bytes and give it a 'map name' of 'test'
                    MemoryMappedFile mmf = MemoryMappedFile.CreateNew("myMappedFile", 1000);

                    string selected = null;

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false); 

                    if (args.Length == 1) //apertura da context menu
                    {
                        selected = args[0];
                        lock (lockerPathSend)
                        {
                            pathSend = selected;
                        }
            
                    }

                    //manage user preference

                    if (File.Exists(Application.StartupPath + @"\\userPreferences.txt"))

                    {
                        //userPref already exists --> load old user Pref
                        try
                        {
                            string[] s = new string[6];
                            s = readUserPreference();
                            if (s[0].Equals("0"))
                            {
                                //default user image is used
                                LANSharingApp.user_image = global::LANSharing.Properties.Resources.user_80;
                                LANSharingApp.user_small_image= global::LANSharing.Properties.Resources.user_32;
                                LANSharingApp.image_flag = 0;
                                LANSharingApp.user_image_path = "no";
                                LANSharingApp.user_small_image_path = "no";
                                Console.WriteLine("Default Image");
                            }
                            else
                            {
                                //LANSharingApp.user_image = Image.FromFile(s[1]);
                                using (var bmpTemp = new Bitmap(s[1]))
                                {
                                    LANSharingApp.user_image = new Bitmap(bmpTemp);
                                }
                                //LANSharingApp.user_small_image = Image.FromFile(s[2]);
                                using (var bmpTemp = new Bitmap(s[2]))
                                {
                                    LANSharingApp.user_small_image = new Bitmap(bmpTemp);
                                }
                                LANSharingApp.image_flag = 1;
                                LANSharingApp.user_image_path = s[1];
                                LANSharingApp.user_small_image_path = s[2];
                                //custom user image is used
                                Console.WriteLine("Custom Image");
                            }
                            if (s[3].Equals("1"))
                            {
                                //sound on
                                LANSharingApp.sysSoundFlag = 1;
                                Console.WriteLine("Sound on");

                            }
                            else
                            {
                                //sound off
                                LANSharingApp.sysSoundFlag = 0;
                                Console.WriteLine("Sound Off");

                            }

                            int savePref = int.Parse(s[4]);
                            saveProfile = savePref;
                            Console.WriteLine("Save pref: "+ saveProfile);

                            lock (lockerPathSave)
                            {
                                LANSharingApp.pathSave = s[5];
                            }
                           
                            Console.WriteLine("Path pref: " + pathSave);


                        }
                        catch (Exception e)
                        {

                            // Let the user know what went wrong.
                            MessageFormError mfe = new MessageFormError("It's not possible to resume userPreferences: default settings are used");
                            mfe.ShowDialog();
                            Console.WriteLine("The userPreference file could not be read");
                            Console.WriteLine(e.Message);
                            LogFile(e.Message, e.ToString(), e.Source);
                            //default user image is used
                            UpdateUserPreference();
                            LANSharingApp.user_image = global::LANSharing.Properties.Resources.user_80;
                            LANSharingApp.user_small_image = global::LANSharing.Properties.Resources.user_32;
                            LANSharingApp.image_flag = 0;
                            LANSharingApp.user_image_path = "no";
                            LANSharingApp.user_small_image_path = "no";
                        }

                    }
                    else
                    {
                        //write default preference
                        WriteUserPreference();
                    }

                    // Console.WriteLine("ALready running: opening " + pathSend);
                   // Application.EnableVisualStyles();
                   // Application.SetCompatibleTextRenderingDefault(false);

                    //create userManagerUnit
                    umu = new UserManagerUnit();

                    //create a ManualResetEvent used from Server's Threads
                    // true -> server start work  ||  false -> server stop work
                    mre = new ManualResetEvent(true);

                    // create Server istance
                    //  active server
                    server = new Server();
                    serverThread = new Thread(server.EntryPoint) { Name = "serverThread" };
                    serverThread.IsBackground = true;
                    serverThread.Start();

                    //create new client
                    client = new Client();
                
                    // create gui istance
                    gui = new GUI(selected);

                    //run application
                    Application.Run(gui);
                    

                }
            }    
        }

            // method used to manage Exception and to write a LogFile
            // the log file is stored int the Bin/Debug/logfile.txt
            public static void LogFile(string sExceptionName, string sEventName, string sControlName)
            {
                try
                {
                    StreamWriter log;


                    if (!File.Exists(Application.StartupPath + @"\\logfile.txt"))
                    {
                        log = new StreamWriter(Application.StartupPath + @"\\logfile.txt");
                    }
                    else
                    {
                        log = File.AppendText(Application.StartupPath + @"\\logfile.txt");
                    }

                    // Write to the file:
                    log.WriteLine("Data Time:" + DateTime.Now);
                    log.WriteLine("Exception Name:" + sExceptionName);
                    log.WriteLine("Event Name:" + sEventName);
                    log.WriteLine("Control Name:" + sControlName);
                    log.WriteLine(Environment.NewLine + "//**********************************//" + Environment.NewLine);

                    // Close the stream:
                    log.Close();
                }
                catch(Exception e)
                {
                     Console.WriteLine(e.Message);
                     closing = true;
                     LANSharingApp.serverThread.Join();
                     clearTempFiles();
                     flushTmpDirectory();
                     Application.Exit();
                }

            }

        public static void WriteUserPreference()
        {
            try
            {
                StreamWriter pref;
                pref = new StreamWriter(Application.StartupPath + @"\\userPreferences.txt");
                // Write to the file default values:
                pref.WriteLine("image_flag " + image_flag);
                pref.WriteLine("user_image_path " + user_image_path);
                pref.WriteLine("user_small_image_path " + user_small_image_path);
                pref.WriteLine("sys_Sound_Flag " + sysSoundFlag);
                pref.WriteLine("save_profile " + saveProfile);
                lock (lockerPathSave)
                {
                    pref.WriteLine("path_save " + pathSave);
                }
                pref.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                LogFile(e.Message, e.ToString(), e.Source);


                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                // delegate the operation on form to the GUI
                MessageFormError mfe = new MessageFormError("An exception has occurred in writing userPreference file");
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.ShowDialog();
                });
                closing = true;
                LANSharingApp.serverThread.Join();
                clearTempFiles();
                flushTmpDirectory();
                Application.Exit();
            }
            
        }

        public static void UpdateUserPreference() {

            try
            {
                StreamWriter pref;

                if (File.Exists(Application.StartupPath + @"\\userPreferences.txt"))
                    File.Delete(Application.StartupPath + @"\\userPreferences.txt");

                pref = new StreamWriter(Application.StartupPath + @"\\userPreferences.txt");
                // Write to the file:
                pref.WriteLine("image_flag " + image_flag);
                pref.WriteLine("user_image_path " + user_image_path);
                pref.WriteLine("user_small_image_path " + user_small_image_path);
                pref.WriteLine("sys_Sound_Flag " + sysSoundFlag);
                pref.WriteLine("save_profile " + saveProfile);
                lock (lockerPathSave)
                {
                    pref.WriteLine("path_save " + pathSave);
                }

                // Close the stream:
                pref.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                LogFile(e.Message, e.ToString(), e.Source);


                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                // delegate the operation on form to the GUI
                MessageFormError mfe = new MessageFormError("An exception has occurred in updating userPreference file");
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.ShowDialog();
                });
                closing = true;
                LANSharingApp.serverThread.Join();
                clearTempFiles();
                flushTmpDirectory();
                Application.Exit();
            }
        }

        public static string[] readUserPreference() {

            try
            {
                StreamReader sr;
                sr = new StreamReader(Application.StartupPath + @"\\userPreferences.txt");

                string[] s = new string[6];
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] pref = line.Split(' ');
                    s[i] = pref[1];
                    i++;
                }

                sr.Close();
                return s;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                LogFile(e.Message, e.ToString(), e.Source);


                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                // delegate the operation on form to the GUI
                MessageFormError mfe = new MessageFormError("An exception has occurred in reading userPreference file");
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.ShowDialog();
                });
                closing = true;
                LANSharingApp.serverThread.Join();
                clearTempFiles();
                flushTmpDirectory();
                Application.Exit();
                return null;
            }
        }

        public static void clearTempFiles()
        {
            try
            {


                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                //delete all temp files generated on receive
                foreach (string toDelete in LANSharingApp.tempFileRecv.ToList<string>())
                {
                    if (File.Exists(toDelete))
                    {
                        File.Delete(toDelete);
                        LANSharingApp.tempFileRecv.Remove(toDelete);
                    }
                    else if (Directory.Exists(toDelete))
                    {
                        Directory.Delete(toDelete);
                        LANSharingApp.tempFileRecv.Remove(toDelete);
                    }
                }
                //delete all temp files generated on send
                foreach (string toDelete in LANSharingApp.tempFileSend.ToList<string>())
                {
                    if (File.Exists(toDelete))
                    {
                        File.Delete(toDelete);
                        LANSharingApp.tempFileSend.Remove(toDelete);
                    }
                    else if (Directory.Exists(toDelete))
                    {
                        Directory.Delete(toDelete);
                        LANSharingApp.tempFileSend.Remove(toDelete);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                LogFile(e.Message, e.ToString(), e.Source);
                closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }

        }
       //delete all tmp generated and not removed in exceptional way
       public static void flushTmpDirectory()
       {

            try
            {

                System.IO.DirectoryInfo di = new DirectoryInfo(LANSharingApp.tmpPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                LogFile(e.Message, e.ToString(), e.Source);

                closing = true;
                LANSharingApp.serverThread.Join();
                Application.Exit();
            }


        }

        


    }
}
