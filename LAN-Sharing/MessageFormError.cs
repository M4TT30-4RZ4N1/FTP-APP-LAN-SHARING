using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LANSharing
{
    public partial class MessageFormError : Form
    {
        public MessageFormError(string message)
        {
            InitializeComponent();
            errorLabel.Text = message;
        }

        private void errorLabel_Click(object sender, EventArgs e)
        {

        }

        private void okbutton_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(okbutton, "Close");
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void disableButton()
        {
            this.okbutton.Visible = false;
        }
    }
}
