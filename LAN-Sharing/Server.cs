using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using static System.IO.Compression.ZipArchive;
using System.Net.NetworkInformation;
using System.Media;
using System.Drawing;
using System.Drawing.Imaging;

namespace LANSharing
{
    class Server
    {
        // define UDP AND TCP threads and Port

        private static Thread threadUDP;
        private static Thread threadTCP;
        private static Thread notifyPresenceUDP;
        private static int FTP_Port_send = 16000;
        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
        static SoundPlayer audio_incoming_file = new SoundPlayer(LANSharing.Properties.Resources.incoming_file); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name

        private static MulticastUdpClient udpClientWrapper;
        //create address object
        private static int port = 1700;
        IPAddress multicastIPaddress = IPAddress.Parse("239.255.255.17");
        IPAddress localIPaddress = LANSharingApp.umu.getAdmin().getIp();

        static readonly object lockerFile = new object(); //each thread server must access to an exclusive section one at time
        static readonly object lockerDir = new object(); //each thread server must access to an exclusive section one at time


        public void EntryPoint()
        {
            try  // start both udp and tcp 
            {
                threadUDP = new Thread(EntryUDP);
                threadUDP.IsBackground = true;
                threadUDP.Start();

                threadTCP = new Thread(EntryTCP);
                threadTCP.IsBackground = true;
                threadTCP.SetApartmentState(ApartmentState.STA);
                threadTCP.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                // delegate the operation on form to the GUI
                MessageFormError mfe = new MessageFormError("UDP or TCP service not available");
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.ShowDialog();
                });
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }

        }

        // EntryUDP start 2 thread: one for notify presence and one for detect other users on the LAN
        public void EntryUDP()
        {
            try
            {
                //create MulticastUdpClient
                udpClientWrapper = new MulticastUdpClient(multicastIPaddress, port, localIPaddress);
                udpClientWrapper.UdpMessageReceived += OnUdpMessageReceived;

                notifyPresenceUDP = new Thread(EntryNotifyPresence);
                notifyPresenceUDP.IsBackground = true;
                notifyPresenceUDP.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }
        }

        void OnUdpMessageReceived(object sender, MulticastUdpClient.UdpMessageReceivedEventArgs e)
        {         
            try
            {
                string receivedText = ASCIIEncoding.ASCII.GetString(e.Buffer);
            
                string[] infoUser = receivedText.Split(','); //conversion
                //Console.WriteLine("UDP- ricevuto un pacchetto.: " + infoUser[0] + " " + infoUser[1]);
               // Console.WriteLine("UDP packet: " + infoUser[0] + ", " + infoUser[1] + ", " + infoUser[2] + " " + infoUser[3] + ", " + infoUser[4] + " " + infoUser[5]);
                Image userImage;
                userImage = Base64ToImage(infoUser[5]);
                if (LANSharingApp.umu.isPresent(infoUser[1] + infoUser[0]))
                {
                    User u = new User(infoUser[0], infoUser[1], infoUser[2], infoUser[3], infoUser[4], userImage); //create new user
                    if ((infoUser[2].CompareTo("online") == 0))
                    {
                      // check if the user is already registered

                        if (!u.isEqual(LANSharingApp.umu.getAdmin()))
                        {
                                LANSharingApp.umu.addUser(u);//add user
                        }

                        LANSharingApp.umu.resetUserTimer(infoUser[1] + infoUser[0]); // reset user timer
                    }
                    else
                    {
                         u.isOld();
                    }
                }
                else // user not in list
                {                     
                            User p = new User(infoUser[0], infoUser[1], infoUser[2], infoUser[3], infoUser[4], userImage); //create new user
                            if (!p.isEqual(LANSharingApp.umu.getAdmin()))
                            {
                                if (infoUser[2].CompareTo("online") == 0) //check not equal to admin
                                {
                                    LANSharingApp.umu.addUser(p);//add user
                                }
                            }
                 }                                 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                LANSharingApp.LogFile(ex.Message, ex.ToString(), ex.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }
        }

        // start to notify wiht UDP packets the online presence
        public void EntryNotifyPresence()
        {
            try
            {
                while (!LANSharingApp.closing)
                {
                    //check ethernet, need this line to generate exception!!!
                    string localIP = LANSharingApp.umu.GetLocalIP();

                    //check if localIP == Admin.getIP
                    if (!localIP.Equals(LANSharingApp.umu.getAdmin().getIp().ToString()))
                    {
                        LANSharingApp.umu.getAdmin().setIP(localIP);
                        Console.WriteLine("Internet Network Changed: Restart Procedure.");
                        // delegate the operation on form to the GUI
                        LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                            Application.Restart();
                        });
                  
                    }

                    // send broadcast every 2s, only if ONLINE
                    if (LANSharingApp.umu.getAdmin().getState().CompareTo("online") == 0) {
                        sendBroadcastPacket(LANSharingApp.umu.getAdmin().getString());
                        //Console.WriteLine("UDP packet Admin: "+ LANSharingApp.umu.getAdmin().getString());
                    }
                      
                    // need a timer
                    Thread.Sleep(2000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);

                LANSharingApp.closing = true;               
                // delegate the operation on form to the GUI
                MessageFormError mfe = new MessageFormError("Error: no lan connection detected, connect to a lan then restart application.");
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.ShowDialog();
                });
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }

        }
        // send udp packets for online-presence
        static void sendBroadcastPacket(string message)
        {
            // there is a problem with d-link router --> need this broadcat, not IPAddress.Broadcast
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Broadcast, FTP_Port_send);

            try
            {
                udpClientWrapper.SendMulticast(ASCIIEncoding.ASCII.GetBytes(message));

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        // EntryPoint of TCP operation
        public void EntryTCP()
        {
            try
            {
                // call only one time, on launch
                if (LANSharingApp.umu.getAdmin().isOnline() && !LANSharingApp.closing)
                    ReceiveFileProcedure();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                LANSharingApp.closing = true;
                LANSharingApp.serverThread.Join();
                LANSharingApp.clearTempFiles();
                LANSharingApp.flushTmpDirectory();
                Application.Exit();
            }

        }

        // It is the real server, each time we receive a new request crete a new thread to handle it
        // use a manualResetEvent to notify when the user is OFFLINE and all servers must stop and reject request
        public void ReceiveFileProcedure()
        {
            TcpListener listener = null;
            BinaryWriter writer = null;
            TcpClient client = null;

            try
            {
                listener = new TcpListener(LANSharingApp.umu.getAdmin().getIp(), LANSharingApp.umu.getAdmin().getPort());// Imposto tcplistener con le credenziali della persona
                listener.Start(); // Istart listen for incoming TCP connections
                Console.WriteLine("[Server] - creazione tcp listener: ");

                int n = 0; // request number, DEBUG purpose
                while (!LANSharingApp.closing)
                {
                    LANSharingApp.mre.WaitOne(); //wait on mre, online or launch app
                    Console.WriteLine("[Server] - in ascolto... ");
                    client = listener.AcceptTcpClient();// blocking

                    if (LANSharingApp.umu.getAdmin().isOnline())
                    {
                        // create a new thread to satisfy the client request
                        n++;
                        Console.WriteLine("[Server] - accettata richiesta #n: " + n);
                        Thread t = new Thread(ProcessClientRequest); //process the client request
                        t.IsBackground = true;
                        t.Start(client);

                    }
                    else
                    {
                        //cancel client request
                        //avoid block on client side, need to comunicate the decision
                        NetworkStream networkStream = client.GetStream();
                        writer = new BinaryWriter(networkStream);
                        writer.Write("cancel");
                        writer.Flush();

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
            }
            finally
            {
                //release resources
                if (listener != null)
                    listener.Stop();
                if (writer != null)
                    writer.Close();
                if (client != null)
                    client.Close();
            }
        }

        // It is the kernel of the FTP protocol Server side
        public void ProcessClientRequest(object argument)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("");
            string msg = "";
            TcpClient client = (TcpClient)argument;
            BinaryWriter writer = null;
            BinaryReader reader = null;
            long check;
            string zipFileName = null;
            string zipDirName = null;

            string newName = null;

            if (!Directory.Exists(LANSharingApp.tmpPath))
                Directory.CreateDirectory(LANSharingApp.tmpPath);

            try
            {
                using (NetworkStream networkStream = client.GetStream())
                {
                    writer = new BinaryWriter(networkStream);
                    reader = new BinaryReader(networkStream);

                    Console.WriteLine("[Server] - aspetto ");
                    msg = reader.ReadString();
                    // receive header
                    // username,usersurname, userip @ type @ path @ checkNumber
                    Console.WriteLine("[Server] - ricevuto header: " + msg);
                    string[] header = msg.Split('@');
                    string[] userInfo = header[0].Split(',');
                    string type = header[1];
                    string fileName = header[2];

                    string currentPathSave = null; //each thread with profile 2 of Save can have different PathSave

                    if (type.CompareTo("file") == 0)
                    {

                        if (LANSharingApp.saveProfile == 1)
                        {   // no automatic save, save on default path
                            if (LANSharingApp.sysSoundFlag == 1)
                            {
                                audio_incoming_file.Play();
                            }

                            lock (LANSharingApp.lockerPathSave)
                            {
                                currentPathSave = LANSharingApp.pathSave;
                            }
                           

                            switch (MessageBox.Show(userInfo[0] + " " + userInfo[1] + " " + userInfo[2] + " wants to send you a file: " + fileName, "Incoming File", MessageBoxButtons.OKCancel))
                            {
                                case DialogResult.Cancel:
                                    writer.Write("cancel");
                                    reader.Close();
                                    writer.Close();
                                    client.Close();
                                    return;
                                case DialogResult.OK:
                                    writer.Write("ok");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (LANSharingApp.saveProfile == 2)
                        {
                            // no automatic save, ask where to save
                            if (LANSharingApp.sysSoundFlag == 1)
                            {
                                audio_incoming_file.Play();
                            }
                            switch (MessageBox.Show(userInfo[0] + " " + userInfo[1] + " " + userInfo[2] + " wants to send you a file: " + fileName, "Incoming File", MessageBoxButtons.OKCancel))
                            {
                                case DialogResult.Cancel:
                                    writer.Write("cancel");
                                    reader.Close();
                                    writer.Close();
                                    client.Close();
                                    return;
                                case DialogResult.OK:
                                    //choose path
                                    // cause thread are MTA on launch
                                    Thread t = new Thread(() =>
                                    {
                                        using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                                        {
                                            DialogResult dr = fbd.ShowDialog();
                                            if (dr == DialogResult.OK)
                                                currentPathSave = fbd.SelectedPath;
                                        }
                                    });
                                    t.IsBackground = true;
                                    // must use STA for the new thread
                                    t.SetApartmentState(ApartmentState.STA);
                                    t.Start();
                                    t.Join();

                                    if(currentPathSave== null)
                                    {
                                        writer.Write("cancel");
                                        reader.Close();
                                        writer.Close();
                                        client.Close();
                                        return;
                                    }
                                    else
                                         writer.Write("ok");
                                    break;
                                default:
                                    break;
                            }
                            //show a message box with browse destination

                        }
                        else
                        {   // automatic save
                            lock (LANSharingApp.lockerPathSave)
                            {
                                currentPathSave = LANSharingApp.pathSave;
                            }
                            writer.Write("ok");
                        }

                        Console.WriteLine("[Server] - Inviato al client decisione: ");
                        // receive file
                        msg = reader.ReadString();
                        long checkNumber = long.Parse(msg);
                        Console.WriteLine("[Server] - checkNumeber: " + checkNumber);

                        // detect duplicates
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.InitialDirectory = currentPathSave;
                                       
                        // get a random file name for zip --> no collision 
                        saveFile.FileName = Path.GetRandomFileName();
                        zipFileName = LANSharingApp.tmpPath + "\\"+ saveFile.FileName;
                        //add to list
                        LANSharingApp.tempFileRecv.Add(zipFileName);
                
                        Console.WriteLine("[Server] - salvataggio in: " + saveFile.FileName);
                        using (Stream output = new FileStream(zipFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                        {
                           // Buffer for reading data
                            Byte[] bytes = new Byte[1024];

                            int length;
                            check = 0;// check if there are no data, in case of delete of file before sending on Client side
                            Console.WriteLine("[Server] - inizio lettura");

                                while ((length = networkStream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    check += length;
                                    output.Write(bytes, 0, length);
                                }
                            }
                            Console.WriteLine("[Server] - fine lettura");
                            Console.WriteLine("[Server] - check:" + check);

                            if (check != checkNumber)//control if something go wrong
                            {
                                File.Delete(zipFileName);
                                MessageFormError mfe = new MessageFormError("Error: missing or corrupted file: " + fileName + "\n" + " from " + userInfo[0] + " " + userInfo[1]);
                                if (LANSharingApp.sysSoundFlag == 1)
                                {
                                    audio_error.Play();
                                }
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                        mfe.Show();
                                    });
                            }
                             else //extract file from zip
                              {

                                //unzip and delete zip archive
                                Console.WriteLine("[Server] - estrazione di:" + zipFileName);
                                Console.WriteLine("[Server] - estrazione in:" + currentPathSave + "\\" + newName);


                                lock (lockerFile)
                                {
                                    newName = GetUniqueNameFile(fileName, currentPathSave); //special function  

                                    //flag used during unzip process
                                    bool duplicate = true;
                                    if (fileName.Equals(newName))
                                    {
                                        duplicate = false; // no duplicate if the name doesn't change
                                    }
                                    //at this time i can have or not a duplicate
                                    // each time i receive a duplicate, move the original as copie N and save the last one as original
                                    if (duplicate)
                                        System.IO.File.Move(currentPathSave + "\\" + fileName, currentPathSave + "\\" + newName);

                                    // Open an existing zip file with random name
                                    ZipStorer zip = ZipStorer.Open(zipFileName, FileAccess.Read);
                                    // Read the central directory collection
                                    List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

                                    foreach (ZipStorer.ZipFileEntry entry in dir) // there is only one entry !!!
                                    {
                                        zip.ExtractFile(entry, currentPathSave + "\\" + fileName);
                                    }

                                    zip.Close();

                                }
                           
                              }

                    }
                    else
                    {
                        //receive folder
                        if (LANSharingApp.saveProfile == 1)
                        {   // no automatic save
                            lock (LANSharingApp.lockerPathSave)
                            {
                                currentPathSave = LANSharingApp.pathSave;
                            }
                            if (LANSharingApp.sysSoundFlag == 1)
                            {
                                audio_incoming_file.Play();
                            }
                            switch (MessageBox.Show(userInfo[0] + " " + userInfo[1] + " " + userInfo[2] + " wants to send you a folder: " + fileName, "Incoming Folder", MessageBoxButtons.OKCancel))
                            {
                                case DialogResult.Cancel:
                                    writer.Write("cancel");
                                    reader.Close();
                                    writer.Close();
                                    client.Close();
                                    return;
                                case DialogResult.OK:
                                    writer.Write("ok");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (LANSharingApp.saveProfile == 2)
                        {
                            // no automatic save, ask where to save
                            audio_incoming_file.Play();
                            switch (MessageBox.Show(userInfo[0] + " " + userInfo[1] + " " + userInfo[2] + " wants to send you a folder: " + fileName, "Incoming Folder", MessageBoxButtons.OKCancel))
                            {
                                case DialogResult.Cancel:
                                    writer.Write("cancel");
                                    reader.Close();
                                    writer.Close();
                                    client.Close();
                                    return;
                                case DialogResult.OK:
                                    //choose path
                                    // cause thread are MTA on launch
                                    Thread t = new Thread(() =>
                                    {
                                        using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                                        {
                                            DialogResult dr = fbd.ShowDialog();
                                            if (dr == DialogResult.OK)
                                                currentPathSave = fbd.SelectedPath;
                                        }
                                    });
                                    t.IsBackground = true;
                                    // must use STA for the new thread
                                    t.SetApartmentState(ApartmentState.STA);
                                    t.Start();
                                    t.Join();

                                    if (currentPathSave == null)
                                    {
                                        writer.Write("cancel");
                                        reader.Close();
                                        writer.Close();
                                        client.Close();
                                        return;
                                    }
                                    else
                                        writer.Write("ok");
                                    break;
                                default:
                                    break;
                            }
                            //show a message box with browse destination
                        }
                        else
                        {   // automatic save
                            lock (LANSharingApp.lockerPathSave)
                            {
                                currentPathSave = LANSharingApp.pathSave;
                            }
                            writer.Write("ok");
                        }

                        Console.WriteLine("[Server] - Inviato al client decisione ");
                        // receive file
                        msg = reader.ReadString();
                        long checkNumber = long.Parse(msg);
                        Console.WriteLine("[Server] - checkNumeber: " + checkNumber);

                            // detect duplicates
                            SaveFileDialog saveFile = new SaveFileDialog();
                            saveFile.InitialDirectory = currentPathSave;
                            string noZip = fileName;
                            string randomName = Path.GetRandomFileName(); //directory zip
                            zipDirName = LANSharingApp.tmpPath + "\\"+ randomName;

                        //add dir random to list
                        LANSharingApp.tempFileRecv.Add(zipDirName);

                      
                            
                            saveFile.FileName = randomName;

                            Console.WriteLine("[Server] - salvataggio in: " + saveFile.FileName);
                            using (Stream output = new FileStream(zipDirName, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                            {
                                // Buffer for reading data
                                Byte[] bytes = new Byte[1024];

                                int length;
                                check = 0;
                                Console.WriteLine("[Server] - inizio lettura zip");
                                while ((length = networkStream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    check += length;
                                    output.Write(bytes, 0, length);
                                }
                            }
                            Console.WriteLine("[Server] - fine lettura zip");
                            Console.WriteLine("[Server] - check: " + check);

                            if (check != checkNumber)//control if something go wrong
                            {
                                File.Delete(zipDirName);
                                MessageFormError mfe = new MessageFormError("Error: missing or corrupted directory: " + fileName + "\n" + " from " + userInfo[0] + " " + userInfo[1]);
                            if (LANSharingApp.sysSoundFlag == 1)
                                {
                                    audio_error.Play();
                                }
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                        mfe.Show();
                                    });
                                    Console.WriteLine("[Server] -  chiusa connessione");
                                    return;
                                }
                                else
                                {
                                    //unzip and delete zip archive
                                    Console.WriteLine("[Server] - estrazione di:" + zipDirName);
                                    Console.WriteLine("[Server] - estrazione in:" + currentPathSave + "\\" + newName);


                                    lock (lockerDir)
                                    {
                                        newName = GetUniqueNameDir(noZip, currentPathSave); //special function

                                        //flag used during unzip process
                                        bool duplicate = true;
                                        if (noZip.Equals(newName))
                                        {
                                            duplicate = false; // no duplicate if the name doesn't change
                                        }
                                        //at this time i can have or not a duplicate
                                        // each time i receive a duplicate, move the original as copie N and save the last one as original
                                        if (duplicate)
                                            Directory.Move(currentPathSave + "\\" + noZip, currentPathSave + "\\" + newName);

                                        ZipFile.ExtractToDirectory(zipDirName, currentPathSave);
                                        File.Delete(zipDirName);// delete zip

                                    }

                                }

                    }

                    Console.WriteLine("[Server] - chiusa connessione");

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                MessageFormError mfe = new MessageFormError("An error has occured during FTP process Server Side");
                audio_error.Play();
                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                });
            }
            finally
            {
                if (zipFileName != null && File.Exists(zipFileName))
                {
                    File.Delete(zipFileName);
                    LANSharingApp.tempFileRecv.Remove(zipFileName);
                }
          
                if(zipDirName!=null && Directory.Exists(zipDirName))
                {
                    File.Delete(zipDirName);
                    LANSharingApp.tempFileRecv.Remove(zipDirName);
                }
                   
                //release resources
                if (writer != null)
                    writer.Close();
                if (reader != null)
                    reader.Close();
                if (client != null)
                    client.Close();
            }

        }

        // method used to create new names for duplicate files
        public string GetUniqueNameFile(string name, string folderPath)
        {
            string pathAndFileName = Path.Combine(folderPath, name);
            string validatedName = name;
            int count = 1;
            while (File.Exists(Path.Combine(folderPath, validatedName)))
            {
                validatedName = string.Format("{0} ({1}){2}",
                    Path.GetFileNameWithoutExtension(pathAndFileName),
                    count++,
                    Path.GetExtension(pathAndFileName));
            }
            return validatedName;
        }

        // method used to create new names for duplicate directory
        public string GetUniqueNameDir(string name, string folderPath)
        {
            string pathAndFileName = Path.Combine(folderPath, name);
            string validatedName = name;
            int count = 1;
            while (Directory.Exists(Path.Combine(folderPath, validatedName)))
            {
                validatedName = string.Format("{0} ({1}){2}",
                    Path.GetFileNameWithoutExtension(pathAndFileName),
                    count++,
                    Path.GetExtension(pathAndFileName));
            }
            return validatedName;
        }


    }
}

