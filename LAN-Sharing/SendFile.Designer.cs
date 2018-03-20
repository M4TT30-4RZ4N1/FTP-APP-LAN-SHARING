namespace LANSharing
{
    partial class SendFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendFile));
            this.progressLabel = new System.Windows.Forms.Label();
            this.compressionLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cancelFTP = new System.Windows.Forms.Button();
            this.progressBar = new LANSharing.NewProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.progressLabel.Location = new System.Drawing.Point(20, 25);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(542, 42);
            this.progressLabel.TabIndex = 1;
            this.progressLabel.Text = "Sending Progress";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressLabel.Click += new System.EventHandler(this.progressLabel_Click);
            // 
            // compressionLabel
            // 
            this.compressionLabel.ForeColor = System.Drawing.Color.White;
            this.compressionLabel.Location = new System.Drawing.Point(24, 136);
            this.compressionLabel.Name = "compressionLabel";
            this.compressionLabel.Size = new System.Drawing.Size(549, 30);
            this.compressionLabel.TabIndex = 3;
            this.compressionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.compressionLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::LANSharing.Properties.Resources.a;
            this.pictureBox1.Location = new System.Drawing.Point(471, 211);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(197, 161);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // cancelFTP
            // 
            this.cancelFTP.BackColor = System.Drawing.Color.White;
            this.cancelFTP.BackgroundImage = global::LANSharing.Properties.Resources.cancel50;
            this.cancelFTP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cancelFTP.Location = new System.Drawing.Point(234, 193);
            this.cancelFTP.Margin = new System.Windows.Forms.Padding(2);
            this.cancelFTP.Name = "cancelFTP";
            this.cancelFTP.Size = new System.Drawing.Size(110, 77);
            this.cancelFTP.TabIndex = 2;
            this.cancelFTP.UseVisualStyleBackColor = false;
            this.cancelFTP.Visible = false;
            this.cancelFTP.Click += new System.EventHandler(this.cancelFTP_Click);
            this.cancelFTP.MouseEnter += new System.EventHandler(this.cancelFTP_MouseEnter);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(24, 87);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(549, 35);
            this.progressBar.TabIndex = 0;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // SendFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(604, 320);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.compressionLabel);
            this.Controls.Add(this.cancelFTP);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SendFile";
            this.Text = "SendFile";
            this.Load += new System.EventHandler(this.SendFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button cancelFTP;
        private System.Windows.Forms.Label compressionLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public NewProgressBar progressBar;
    }
}