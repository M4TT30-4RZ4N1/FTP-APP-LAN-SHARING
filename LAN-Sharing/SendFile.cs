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

namespace LANSharing
{
    public partial class SendFile : Form
    {
        //Token set by the cancel clik
        // when it's modified, the client stop the sending process and terminate
        public CancellationTokenSource cts = new CancellationTokenSource();

        public SendFile(int min, int max, string textToDisplay)
        {
            InitializeComponent();
            progressLabel.Text = textToDisplay;
            progressBar.Minimum = min;
            progressBar.Maximum= max;
    
                    
        }

        public SendFile(string textToDisplay, string compressionMessage)
        {
            InitializeComponent();
            progressLabel.Text = textToDisplay;
            compressionLabel.Text = compressionMessage;

        }

        public void setMinMaxBar(int min, int max)
        {
            progressBar.Minimum = min;
            progressBar.Maximum = max;
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        public void clearCompression()
        {
            cancelFTP.Visible = true;
            compressionLabel.Text = "Compression Finished! FTP running...";
        }

        public void errorFTP()
        {
            compressionLabel.Text = "A problem has occured, FTP canceled!";
        }

        public void endFTP()
        {
            compressionLabel.Text = "FTP ended with success!";
        }

        public void incrementProgressBar()
        {                 
                progressBar.Increment(1);     
        }

        private void cancelFTP_Click(object sender, EventArgs e)
        {        
                //cancel the operation if the progress bar has not finish
                cts.Cancel();            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressLabel_Click(object sender, EventArgs e)
        {

        }

        private void cancelFTP_MouseEnter(object sender, EventArgs e)
        {
                toolTip1.ShowAlways = true;
                toolTip1.SetToolTip(cancelFTP, "Stop Sending");
           
        }

        private void SendFile_Load(object sender, EventArgs e)
        {

        }
        //enable form movable
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

    }

}
