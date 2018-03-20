using System.Drawing;

namespace LANSharing
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.defaultPath = new System.Windows.Forms.Label();
            this.destinationPath = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.radioButtonNoAutomaticDefault = new System.Windows.Forms.RadioButton();
            this.radioButtonAutomaticSave = new System.Windows.Forms.RadioButton();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.radioButtonNoAutomaticComplete = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.FirstName = new MetroFramework.Controls.MetroLabel();
            this.LastName = new MetroFramework.Controls.MetroLabel();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.sound = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.soundTile = new MetroFramework.Controls.MetroTile();
            this.user_tile = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // defaultPath
            // 
            this.defaultPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.defaultPath.AutoSize = true;
            this.defaultPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultPath.Location = new System.Drawing.Point(34, 411);
            this.defaultPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.defaultPath.Name = "defaultPath";
            this.defaultPath.Size = new System.Drawing.Size(202, 25);
            this.defaultPath.TabIndex = 1;
            this.defaultPath.Text = "Default Save Path\r\n";
            this.defaultPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // destinationPath
            // 
            this.destinationPath.AccessibleName = "destinationPath";
            this.destinationPath.AutoSize = true;
            this.destinationPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destinationPath.Location = new System.Drawing.Point(34, 446);
            this.destinationPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.destinationPath.Name = "destinationPath";
            this.destinationPath.Size = new System.Drawing.Size(0, 20);
            this.destinationPath.TabIndex = 2;
            this.destinationPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.AccessibleName = "browse";
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(39, 508);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 32);
            this.button1.TabIndex = 4;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.AccessibleName = "saveButton";
            this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveButton.Location = new System.Drawing.Point(606, 537);
            this.saveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(111, 38);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.AccessibleName = "cancel";
            this.Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Cancel.Location = new System.Drawing.Point(744, 537);
            this.Cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(126, 38);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // radioButtonNoAutomaticDefault
            // 
            this.radioButtonNoAutomaticDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonNoAutomaticDefault.AutoSize = true;
            this.radioButtonNoAutomaticDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNoAutomaticDefault.Location = new System.Drawing.Point(39, 320);
            this.radioButtonNoAutomaticDefault.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonNoAutomaticDefault.Name = "radioButtonNoAutomaticDefault";
            this.radioButtonNoAutomaticDefault.Size = new System.Drawing.Size(694, 29);
            this.radioButtonNoAutomaticDefault.TabIndex = 10;
            this.radioButtonNoAutomaticDefault.Text = "Save Interaction Default (save data on the default path with user permission)";
            this.radioButtonNoAutomaticDefault.UseVisualStyleBackColor = true;
            this.radioButtonNoAutomaticDefault.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox_Click);
            // 
            // radioButtonAutomaticSave
            // 
            this.radioButtonAutomaticSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonAutomaticSave.AutoSize = true;
            this.radioButtonAutomaticSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAutomaticSave.Location = new System.Drawing.Point(39, 283);
            this.radioButtonAutomaticSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonAutomaticSave.Name = "radioButtonAutomaticSave";
            this.radioButtonAutomaticSave.Size = new System.Drawing.Size(647, 29);
            this.radioButtonAutomaticSave.TabIndex = 11;
            this.radioButtonAutomaticSave.Text = "Automatic Save (save data on the default path without user interaction)";
            this.radioButtonAutomaticSave.UseVisualStyleBackColor = true;
            this.radioButtonAutomaticSave.CheckedChanged += new System.EventHandler(this.radioButtonEnabled_CheckedChanged);
            this.radioButtonAutomaticSave.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox_Click);
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.AccessibleName = "FirstName";
            this.textBoxFirstName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxFirstName.Location = new System.Drawing.Point(302, 135);
            this.textBoxFirstName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.ReadOnly = true;
            this.textBoxFirstName.Size = new System.Drawing.Size(174, 26);
            this.textBoxFirstName.TabIndex = 15;
            this.textBoxFirstName.TextChanged += new System.EventHandler(this.textBoxFistName_TextChanged);
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.AccessibleName = "LastName";
            this.textBoxLastName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxLastName.Location = new System.Drawing.Point(302, 182);
            this.textBoxLastName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.ReadOnly = true;
            this.textBoxLastName.Size = new System.Drawing.Size(174, 26);
            this.textBoxLastName.TabIndex = 16;
            this.textBoxLastName.TextChanged += new System.EventHandler(this.textBoxLastName_TextChanged);
            // 
            // radioButtonNoAutomaticComplete
            // 
            this.radioButtonNoAutomaticComplete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonNoAutomaticComplete.AutoSize = true;
            this.radioButtonNoAutomaticComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNoAutomaticComplete.Location = new System.Drawing.Point(39, 355);
            this.radioButtonNoAutomaticComplete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonNoAutomaticComplete.Name = "radioButtonNoAutomaticComplete";
            this.radioButtonNoAutomaticComplete.Size = new System.Drawing.Size(708, 29);
            this.radioButtonNoAutomaticComplete.TabIndex = 18;
            this.radioButtonNoAutomaticComplete.Text = "Save Interaction Dynamic (save data on a dynamic path with user permission)";
            this.radioButtonNoAutomaticComplete.UseVisualStyleBackColor = true;
            this.radioButtonNoAutomaticComplete.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.textBox1.Location = new System.Drawing.Point(30, 231);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.MaximumSize = new System.Drawing.Size(825, 20);
            this.textBox1.MinimumSize = new System.Drawing.Size(825, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(825, 19);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = "................................................................................." +
    "................................................................................" +
    ".......................";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.textBox2.Location = new System.Drawing.Point(30, 391);
            this.textBox2.Margin = new System.Windows.Forms.Padding(0);
            this.textBox2.MaximumSize = new System.Drawing.Size(825, 20);
            this.textBox2.MinimumSize = new System.Drawing.Size(825, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(825, 19);
            this.textBox2.TabIndex = 20;
            this.textBox2.Text = "................................................................................." +
    "................................................................................" +
    ".......................";
            // 
            // FirstName
            // 
            this.FirstName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FirstName.AutoSize = true;
            this.FirstName.Location = new System.Drawing.Point(30, 135);
            this.FirstName.Margin = new System.Windows.Forms.Padding(0);
            this.FirstName.MaximumSize = new System.Drawing.Size(180, 62);
            this.FirstName.MinimumSize = new System.Drawing.Size(90, 31);
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(73, 19);
            this.FirstName.TabIndex = 22;
            this.FirstName.Text = "First Name";
            this.FirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LastName
            // 
            this.LastName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LastName.AutoSize = true;
            this.LastName.Location = new System.Drawing.Point(30, 182);
            this.LastName.Margin = new System.Windows.Forms.Padding(0);
            this.LastName.MaximumSize = new System.Drawing.Size(180, 62);
            this.LastName.MinimumSize = new System.Drawing.Size(90, 31);
            this.LastName.Name = "LastName";
            this.LastName.Size = new System.Drawing.Size(71, 19);
            this.LastName.TabIndex = 23;
            this.LastName.Text = "Last Name";
            this.LastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LastName.Click += new System.EventHandler(this.metroLabel1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.label1.Location = new System.Drawing.Point(30, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.MaximumSize = new System.Drawing.Size(300, 31);
            this.label1.MinimumSize = new System.Drawing.Size(300, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 19);
            this.label1.TabIndex = 24;
            this.label1.Text = "Admin Credentials";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.label3.Location = new System.Drawing.Point(30, 251);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.MaximumSize = new System.Drawing.Size(300, 31);
            this.label3.MinimumSize = new System.Drawing.Size(300, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 19);
            this.label3.TabIndex = 25;
            this.label3.Text = "Save Options";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Click += new System.EventHandler(this.metroLabel1_Click_1);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(774, 135);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 57);
            this.button2.TabIndex = 26;
            this.button2.Text = "Clear image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // sound
            // 
            this.sound.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.sound.AutoSize = true;
            this.sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sound.Location = new System.Drawing.Point(579, 411);
            this.sound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sound.Name = "sound";
            this.sound.Size = new System.Drawing.Size(245, 25);
            this.sound.TabIndex = 27;
            this.sound.Text = "Enable/Disable Sound";
            this.sound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soundTile
            // 
            this.soundTile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.soundTile.AutoSize = true;
            this.soundTile.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.soundTile.Location = new System.Drawing.Point(678, 446);
            this.soundTile.Margin = new System.Windows.Forms.Padding(0);
            this.soundTile.MaximumSize = new System.Drawing.Size(60, 62);
            this.soundTile.MinimumSize = new System.Drawing.Size(60, 62);
            this.soundTile.Name = "soundTile";
            this.soundTile.Size = new System.Drawing.Size(60, 62);
            this.soundTile.Style = MetroFramework.MetroColorStyle.White;
            this.soundTile.TabIndex = 28;
            this.soundTile.TileImage = global::LANSharing.Properties.Resources.sound_on40;
            this.soundTile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.soundTile.UseTileImage = true;
            this.soundTile.Click += new System.EventHandler(this.soundTile_Click);
            this.soundTile.MouseEnter += new System.EventHandler(this.soundTile_MouseEnter);
            // 
            // user_tile
            // 
            this.user_tile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.user_tile.AutoSize = true;
            this.user_tile.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.user_tile.Location = new System.Drawing.Point(642, 92);
            this.user_tile.Margin = new System.Windows.Forms.Padding(0);
            this.user_tile.MaximumSize = new System.Drawing.Size(128, 131);
            this.user_tile.MinimumSize = new System.Drawing.Size(128, 131);
            this.user_tile.Name = "user_tile";
            this.user_tile.Size = new System.Drawing.Size(128, 131);
            this.user_tile.Style = MetroFramework.MetroColorStyle.White;
            this.user_tile.TabIndex = 21;
            //this.user_tile.TileImage = global::LANSharing.Properties.Resources.user_80;
            this.user_tile.TileImage = LANSharingApp.user_image;
            this.user_tile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.user_tile.UseTileImage = true;
            this.user_tile.Click += new System.EventHandler(this.user_tile_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 615);
            this.Controls.Add(this.soundTile);
            this.Controls.Add(this.sound);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LastName);
            this.Controls.Add(this.FirstName);
            this.Controls.Add(this.user_tile);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioButtonNoAutomaticComplete);
            this.Controls.Add(this.textBoxLastName);
            this.Controls.Add(this.textBoxFirstName);
            this.Controls.Add(this.radioButtonAutomaticSave);
            this.Controls.Add(this.radioButtonNoAutomaticDefault);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.destinationPath);
            this.Controls.Add(this.defaultPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(15, 92, 15, 15);
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label defaultPath;
        private System.Windows.Forms.Label destinationPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.RadioButton radioButtonNoAutomaticDefault;
        private System.Windows.Forms.RadioButton radioButtonAutomaticSave;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.RadioButton radioButtonNoAutomaticComplete;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        public MetroFramework.Controls.MetroTile user_tile;
        public MetroFramework.Controls.MetroLabel FirstName;
        public MetroFramework.Controls.MetroLabel LastName;
        public MetroFramework.Controls.MetroLabel label1;
        public MetroFramework.Controls.MetroLabel label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label sound;
        public MetroFramework.Controls.MetroTile soundTile;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}