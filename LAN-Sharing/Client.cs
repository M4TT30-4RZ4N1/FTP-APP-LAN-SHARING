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
using System.Net.NetworkInformation;
using System.IO.Compression;
using System.Media;

namespace LANSharing
{
    class Client
    {
        static SoundPlayer audio_error = new SoundPlayer(LANSharing.Properties.Resources.Computer_Error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name

        public void EntryPoint(string userInfo)
        {

            // retrieve user credentials
            string[] credentials = userInfo.Split(',');

            try
            {                
                User user = new User();

                // firstname and lastname
                LANSharingApp.umu.getOnlineUsers().TryGetValue(credentials[1] + credentials[0], out user);

                if (user.isOnline())
                    FTP_protocol(credentials[2], credentials[3], credentials[0], credentials[1]);// Ip and port
                else
                {
                    MessageFormError mfe = new MessageFormError(credentials[1] + " " + credentials[0] + " is offline!");
                    if (LANSharingApp.sysSoundFlag == 1)
                    {
                        audio_error.Play();
                    }
                    // delegate the operation on form to the GUI
                    LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                        mfe.Show();
                    });
                }

            }
            catch(Exception e)
            {
                // null reference to user
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);

                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                MessageFormError mfe = new MessageFormError(credentials[1] + " " + credentials[0] + " is offline!");
               
                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                });
            }


        }

        //kernel of protocol Client side
        private static void FTP_protocol(string ip, string port, string firstNameReceiver, string lastnameReceiver)
        {
            Console.WriteLine("[Client] - Inizio invio a "+ ip+":"+port+" nome: "+ firstNameReceiver + " cognome: "+ lastnameReceiver);
            byte[] buffer = new byte[1024];
            string msg = "";
            string msg_progress = "";
            byte[] buffer2 = new byte[1024];
            TcpClient client = new TcpClient();
            BinaryWriter writer = null;
            BinaryReader reader = null;
            string zipDir = null;
            SendFile windowSendFile= null;
            ZipStorer zipFile = null;
            string zipFileName = null;

            if (!Directory.Exists(LANSharingApp.tmpPath))
                Directory.CreateDirectory(LANSharingApp.tmpPath);


            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                string type = null;
                Console.WriteLine("[Client] - tentativo invio a:"+ ip+":"+ port + " nome: " + firstNameReceiver + " cognome: " + lastnameReceiver);
                
                client.ReceiveBufferSize = 1024;
                client.SendBufferSize = 1024;
                Console.WriteLine("[Client] - creato tcp client");
                //start connection TCP
                client.Connect(IPAddress.Parse(ip), int.Parse(port));
                Console.WriteLine("[Client] - connesso tcp client");
                using (NetworkStream networkStream = client.GetStream())
                {
                    writer = new BinaryWriter(networkStream);
                    reader = new BinaryReader(networkStream);

                    // send header to the Server
                    // userFirstName,userLastName, userIP @ type @ path
                    string userFirstName = LANSharingApp.umu.getAdmin().getFirstName();
                    string userLastName = LANSharingApp.umu.getAdmin().getLastName();
                    string userIp = LANSharingApp.umu.getAdmin().getIp().ToString();
                    string myPath = null;
                    string fullPath = null;

                    lock (LANSharingApp.lockerPathSend)
                    {
                        myPath = Path.GetFileName(LANSharingApp.pathSend);
                        fullPath = LANSharingApp.pathSend;
                    }
                    

                    //identify if the admin is sending a file or a directory
                    if ((File.GetAttributes(fullPath) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        type = "directory";
                    }
                    else
                    {
                        type = "file";
                    }

                    // case file
                    if (type.CompareTo("file") == 0)
                    {             
                        // username,usersurname, userip @ type @ path @ checkNumber
                        //msg_progress = nameReceiver + "," + lastnameReceiver + "," + ip + "@" + type + "@" + myPath + "@" + dataToSend.Length;
                        msg = userFirstName + "," + userLastName + "," + userIp + "@" + type + "@" + myPath;
                        writer.Write(msg);
                        Console.WriteLine("[Client] - Inviato al server: " + msg);
                        Console.WriteLine("[Client] - aspetto risposta");

                        //wait for answer
                        msg = reader.ReadString();
                        Console.WriteLine("[Client] - risposta ricevuta: " + msg);
                        if (msg.CompareTo("ok") == 0)
                        { //if ok, send file
                            //check file
                            if(!File.Exists(fullPath))
                            {
                                MessageFormError mfe = new MessageFormError("Error: file deleted");
                                if (LANSharingApp.sysSoundFlag == 1)
                                {
                                    audio_error.Play();
                                }
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    mfe.Show();
                                });
                                return;

                            }

                            Console.WriteLine("[Client] - inizio invio file ");

                            //zip the file
                            string randomName= LANSharingApp.tmpPath+"\\"+ Path.GetRandomFileName();

                            //add file to list
                            LANSharingApp.tempFileSend.Add(randomName);

                            //specific progress bar for each different user
                            windowSendFile = new SendFile("Progress of ftp file " + myPath + " to " + firstNameReceiver + " " + lastnameReceiver,"Compression in Progress");
                            windowSendFile.StartPosition = FormStartPosition.CenterScreen;
                            int offset = 0;

                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                if (windowSendFile != null)
                                    windowSendFile.Show();
                            });

                            zipFileName = randomName;
                            zipFile = ZipStorer.Create(randomName,"");
                            zipFile.AddFile(ZipStorer.Compression.Store, fullPath, Path.GetFileName(myPath), "");
                            zipFile.Close();
                                            
                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                if (windowSendFile != null)
                                    windowSendFile.clearCompression();
                            });


                            //some usefull information
                            byte[] dataToSend = File.ReadAllBytes(randomName);
                            msg = dataToSend.Length + "";
                            writer.Write(msg);
                            int chunk = 1024 * 1024;
                            int n = dataToSend.Length / chunk;
                            int lastChunk = dataToSend.Length - (n * chunk);
                            int i;

                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                if (windowSendFile != null)
                                    windowSendFile.setMinMaxBar(0,n+1);
                            });


                            for (i = 0; i < n; i++)
                            {
                                if (windowSendFile.cts.IsCancellationRequested)
                                { //manage cancel operation
                                    Console.WriteLine("[Client] - invio annullato ");
                                    // delegate the operation on form to the GUI
                                    LANSharingApp.gui.Invoke((MethodInvoker)delegate ()
                                    {
                                        if (windowSendFile != null)
                                        {
                                            windowSendFile.Dispose();
                                            windowSendFile.Close();
                                        }
                                        
                                    });

                                    return;
                                }
                                else
                                {  // no cancel
                                    networkStream.Write(dataToSend, offset, chunk);
                                    networkStream.Flush();
                                    offset += chunk;
                                    // delegate the operation on form to the GUI
                                    LANSharingApp.gui.Invoke((MethodInvoker)delegate ()
                                    {
                                        if (windowSendFile != null)
                                            windowSendFile.incrementProgressBar();
                                    });
                                }
                          
                            }

                            Thread.Sleep(5000); // give time to user to react
                            if (windowSendFile.cts.IsCancellationRequested)
                            { //manage cancel operation
                                Console.WriteLine("[Client] - invio annullato ");
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate ()
                                {
                                    if (windowSendFile != null)
                                    {
                                        windowSendFile.Dispose();
                                        windowSendFile.Close();
                                    }
                                });

                                return;
                            }

                            if (lastChunk != 0)
                                {
                                    networkStream.Write(dataToSend, offset, lastChunk);
                                    networkStream.Flush();
                                    
                            }

                         
                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                if (windowSendFile != null)
                                    windowSendFile.incrementProgressBar();
                            });

                            Console.WriteLine("[Client] - fine invio file ");
                            Console.WriteLine("[Client] - close protocol ");
                        }
                        else//cancel
                        {
                            Console.WriteLine("[Client] - close protocol ");
                            MessageFormError mfex = new MessageFormError(firstNameReceiver + " " + lastnameReceiver+" refused file");
                            if (LANSharingApp.sysSoundFlag == 1)
                            {
                                audio_error.Play();
                            }
                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                mfex.Show();
                            });
                        }

                        Thread.Sleep(1000); // give time to sender to see the final state of progress bar

                        // delegate the operation on form to the GUI --> close the send window
                        if (windowSendFile!=null) {
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                windowSendFile.Close();
                            });
                        } 
                    }
                    else   //case directory
                    {
                        // username,usersurname, userip @ type @ path
                        msg = userFirstName + "," + userLastName + "," + userIp + "@" + type + "@" + myPath;
                        writer.Write(msg);
                        Console.WriteLine("[Client] - Inviato al server: " + msg);
                        Console.WriteLine("[Client] - aspetto risposta");

                        //wait answer
                        msg = reader.ReadString();
                        Console.WriteLine("[Client] - risposta ricevuta: " + msg);

                        if (msg.CompareTo("ok") == 0)
                        { // if ok, send directory

                           if(!Directory.Exists(fullPath))
                            {
                                MessageFormError mfe = new MessageFormError("Error: directory deleted during send process");
                                if (LANSharingApp.sysSoundFlag == 1)
                                {
                                    audio_error.Play();
                                }
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    mfe.Show();
                                });
                                return;
                            }

                           
                                //zip directory, better performance on LAN
                                //random name, no collision on creation multiple zip of same file
                                zipDir = LANSharingApp.tmpPath+"\\"+ Path.GetRandomFileName();

                            //add to list
                            LANSharingApp.tempFileSend.Add(zipDir);

                                //specific progress bar for each different user
                                windowSendFile = new SendFile("Progress of ftp directory " + myPath + " to " + firstNameReceiver + " " + lastnameReceiver, " Compression in progress");
                                windowSendFile.StartPosition = FormStartPosition.CenterScreen;
                                int offset = 0;

                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    if (windowSendFile != null)
                                        windowSendFile.Show();
                                });

                                ZipFile.CreateFromDirectory(fullPath, zipDir, CompressionLevel.NoCompression, true);
                                Console.WriteLine("[Client] - zip creato:" + zipDir);
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    if (windowSendFile != null)
                                        windowSendFile.clearCompression();
                                });

                                Console.WriteLine("[Client] - inizio invio directory zip");

                                byte[] dataToSend = File.ReadAllBytes(zipDir);
                                msg =  dataToSend.Length + "";
                                writer.Write(msg);
                                int chunk = 1024 * 1024;
                                int n = dataToSend.Length / chunk;
                                int lastChunk = dataToSend.Length - (n * chunk);
                                int i;

                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    if(windowSendFile!=null)
                                        windowSendFile.setMinMaxBar(0, n + 1);
                                });

                                for (i = 0; i < n; i++)
                                {
                                    if (windowSendFile.cts.IsCancellationRequested)
                                    { //manage cancel operation
                                        Console.WriteLine("[Client] - invio annullato ");
                                        // delegate the operation on form to the GUI
                                        LANSharingApp.gui.Invoke((MethodInvoker)delegate ()
                                        {
                                            if (windowSendFile != null)
                                            { 
                                                windowSendFile.Dispose();
                                                windowSendFile.Close();
                                            }
                                        });

                                        return;
                                    }
                                    else
                                    {  // no cancel
                                        networkStream.Write(dataToSend, offset, chunk);
                                        networkStream.Flush();
                                        offset += chunk;
                                        // delegate the operation on form to the GUI
                                        LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                            if (windowSendFile != null)
                                                windowSendFile.incrementProgressBar();
                                        });
                                    }                                 
                                }

                                Thread.Sleep(5000); // give time to user to react
                                if (windowSendFile.cts.IsCancellationRequested)
                                { //manage cancel operation
                                    Console.WriteLine("[Client] - invio annullato ");
                                    // delegate the operation on form to the GUI
                                    LANSharingApp.gui.Invoke((MethodInvoker)delegate ()
                                    {
                                        if (windowSendFile != null)
                                        {
                                            windowSendFile.Dispose();
                                            windowSendFile.Close();
                                        }
                                            
                                    });

                                    return;
                                }

                                if (lastChunk != 0)
                                {
                                    networkStream.Write(dataToSend, offset, lastChunk);
                                    networkStream.Flush();
                                }
                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    if (windowSendFile != null)
                                        windowSendFile.incrementProgressBar();
                                });
    
                                Console.WriteLine("[Client] - fine invio directory zip ");
                                //File.Delete(zipDir);
                                Console.WriteLine("[Client] - close protocol ");

                                // delegate the operation on form to the GUI
                                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                    if (windowSendFile != null)
                                        windowSendFile.endFTP();
                                });


                        }
                        else // if cancel, close
                        {
                               Console.WriteLine("[Client] - close protocol ");
                            MessageFormError mfex = new MessageFormError(firstNameReceiver + " " + lastnameReceiver+" refused file");
                            if (LANSharingApp.sysSoundFlag == 1)
                            {
                                audio_error.Play();
                            }
                            // delegate the operation on form to the GUI
                            LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                                mfex.Show();
                            });
                        }
                        Thread.Sleep(1000); // give time to sender to see the final state of progress bar

                        // delegate the operation on form to the GUI --> close the send window
                        LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                            if (windowSendFile != null)
                                windowSendFile.Close();
                        });
                    }

                    Console.WriteLine("[Client] - chiusa connessione");
                }
            }
            catch(System.IO.IOException cancelException)
            {
                Console.WriteLine(cancelException.ToString());
                LANSharingApp.LogFile(cancelException.Message, cancelException.ToString(), cancelException.Source);
                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                MessageFormError mfe = new MessageFormError("The operation was canceled");
        
                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                    if (windowSendFile != null)
                        windowSendFile.errorFTP();
                });

                Thread.Sleep(1000);

                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                    if (windowSendFile != null)
                    {
                        windowSendFile.Dispose();
                        windowSendFile.Close();
                    }
                        
                });


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                LANSharingApp.LogFile(e.Message, e.ToString(), e.Source);
                if (LANSharingApp.sysSoundFlag == 1)
                {
                    audio_error.Play();
                }
                MessageFormError mfe = new MessageFormError("The selected user is offline, is not possible to satisfy your request");
             
                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                    if (windowSendFile != null)
                        windowSendFile.errorFTP();
                });
                Thread.Sleep(1000);

                // delegate the operation on form to the GUI
                LANSharingApp.gui.Invoke((MethodInvoker)delegate () {
                    mfe.Show();
                    if (windowSendFile != null)
                    {
                        windowSendFile.Dispose();
                        windowSendFile.Close();
                    }

                });


            }
            finally
            {   //release resources
                if (File.Exists(zipFileName))
                {
                    File.Delete(zipFileName);
                    LANSharingApp.tempFileSend.Remove(zipFileName);
                } 
                if (zipDir != null)
                {
                    File.Delete(zipDir);
                    LANSharingApp.tempFileSend.Remove(zipDir);
                }
                
                if (writer != null)
                    writer.Close();
                if (reader != null)
                    reader.Close();
                if (client != null)
                    client.Close();                
            }
        }
    }
}
