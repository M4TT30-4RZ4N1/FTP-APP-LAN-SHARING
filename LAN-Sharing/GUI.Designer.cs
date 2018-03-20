namespace LANSharing
{
  public  partial class GUI : MetroFramework.Forms.MetroForm
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

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.taskbarIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.firstNameLabel = new MetroFramework.Controls.MetroLabel();
            this.lastNameLabel = new MetroFramework.Controls.MetroLabel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.description = new MetroFramework.Controls.MetroLabel();
            this.contextMenuTaskBar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.apriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.stateLabel = new MetroFramework.Controls.MetroLabel();
            this.toolTipShareDirectory = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.shareButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.state_offline_dot = new System.Windows.Forms.Label();
            this.state_online_dot = new System.Windows.Forms.Label();
            this.user_tile = new MetroFramework.Controls.MetroTile();
            this.browseFile = new System.Windows.Forms.Button();
            this.browseDirectory = new System.Windows.Forms.Button();
            this.settingsButton = new MetroFramework.Controls.MetroTile();
            this.refreshButton = new MetroFramework.Controls.MetroTile();
            this.contextMenuTaskBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // taskbarIcon
            // 
            this.taskbarIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.taskbarIcon.BalloonTipText = "App in background!\r\n";
            this.taskbarIcon.BalloonTipTitle = "DoubleClick to open!";
            this.taskbarIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskbarIcon.Icon")));
            this.taskbarIcon.Text = "LANSharing";
            this.taskbarIcon.Visible = true;
            this.taskbarIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.taskbarIcon_MouseDoubleClick);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(677, 145);
            this.firstNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.firstNameLabel.MaximumSize = new System.Drawing.Size(150, 20);
            this.firstNameLabel.MinimumSize = new System.Drawing.Size(75, 20);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(73, 19);
            this.firstNameLabel.TabIndex = 6;
            this.firstNameLabel.Text = "First Name";
            this.firstNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(678, 165);
            this.lastNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lastNameLabel.MaximumSize = new System.Drawing.Size(150, 20);
            this.lastNameLabel.MinimumSize = new System.Drawing.Size(75, 20);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(71, 19);
            this.lastNameLabel.TabIndex = 7;
            this.lastNameLabel.Text = "Last Name";
            this.lastNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lastNameLabel.Click += new System.EventHandler(this.lastName_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.BackColor = System.Drawing.Color.AliceBlue;
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(50, 388);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel.MaximumSize = new System.Drawing.Size(1400, 100);
            this.flowLayoutPanel.MinimumSize = new System.Drawing.Size(700, 100);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(700, 100);
            this.flowLayoutPanel.TabIndex = 11;
            // 
            // description
            // 
            this.description.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(50, 368);
            this.description.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.description.MaximumSize = new System.Drawing.Size(460, 0);
            this.description.MinimumSize = new System.Drawing.Size(230, 0);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(237, 19);
            this.description.TabIndex = 12;
            this.description.Text = "Online users on LAN-Sharing platform:";
            this.description.Click += new System.EventHandler(this.description_Click);
            // 
            // contextMenuTaskBar
            // 
            this.contextMenuTaskBar.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.contextMenuTaskBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuTaskBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.apriToolStripMenuItem,
            this.settingsStripMenuItem,
            this.stateMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem});
            this.contextMenuTaskBar.Name = "contextMenuStripTaskbarIcon";
            this.contextMenuTaskBar.Size = new System.Drawing.Size(124, 96);
            this.contextMenuTaskBar.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripTaskbarIcon_Opening);
            // 
            // apriToolStripMenuItem
            // 
            this.apriToolStripMenuItem.Font = new System.Drawing.Font("Open Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apriToolStripMenuItem.Name = "apriToolStripMenuItem";
            this.apriToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.apriToolStripMenuItem.Text = "Open";
            this.apriToolStripMenuItem.Click += new System.EventHandler(this.openIconContextMenu_Click);
            // 
            // settingsStripMenuItem
            // 
            this.settingsStripMenuItem.Font = new System.Drawing.Font("Open Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsStripMenuItem.Name = "settingsStripMenuItem";
            this.settingsStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.settingsStripMenuItem.Text = "Settings";
            this.settingsStripMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            // 
            // stateMenuItem
            // 
            this.stateMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineMenuItem,
            this.offlineMenuItem});
            this.stateMenuItem.Font = new System.Drawing.Font("Open Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stateMenuItem.Name = "stateMenuItem";
            this.stateMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.stateMenuItem.Size = new System.Drawing.Size(123, 20);
            this.stateMenuItem.Text = "State";
            // 
            // onlineMenuItem
            // 
            this.onlineMenuItem.Checked = true;
            this.onlineMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onlineMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.onlineMenuItem.Name = "onlineMenuItem";
            this.onlineMenuItem.Size = new System.Drawing.Size(113, 22);
            this.onlineMenuItem.Text = "Online";
            this.onlineMenuItem.Click += new System.EventHandler(this.onlineIconContextMenu_Click);
            // 
            // offlineMenuItem
            // 
            this.offlineMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.offlineMenuItem.Name = "offlineMenuItem";
            this.offlineMenuItem.Size = new System.Drawing.Size(113, 22);
            this.offlineMenuItem.Text = "Offline";
            this.offlineMenuItem.Click += new System.EventHandler(this.offlineIconContextMenu_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Open Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // pathBox
            // 
            this.pathBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pathBox.BackColor = System.Drawing.Color.AliceBlue;
            this.pathBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pathBox.Location = new System.Drawing.Point(50, 328);
            this.pathBox.MaximumSize = new System.Drawing.Size(1400, 20);
            this.pathBox.MinimumSize = new System.Drawing.Size(700, 20);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(700, 20);
            this.pathBox.TabIndex = 15;
            // 
            // stateLabel
            // 
            this.stateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(604, 69);
            this.stateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.stateLabel.MaximumSize = new System.Drawing.Size(120, 40);
            this.stateLabel.MinimumSize = new System.Drawing.Size(60, 20);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(60, 19);
            this.stateLabel.TabIndex = 18;
            this.stateLabel.Text = "OFFLINE";
            this.stateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.stateLabel.Click += new System.EventHandler(this.metroLabel1_Click);
            // 
            // toolTipShareDirectory
            // 
            this.toolTipShareDirectory.BackColor = System.Drawing.SystemColors.HighlightText;
            this.toolTipShareDirectory.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 306);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.MaximumSize = new System.Drawing.Size(460, 0);
            this.label1.MinimumSize = new System.Drawing.Size(230, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 19);
            this.label1.TabIndex = 22;
            this.label1.Text = "Select the file/folder to send:";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // shareButton
            // 
            this.shareButton.AccessibleName = "shareButton";
            this.shareButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.shareButton.AutoSize = true;
            this.shareButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.shareButton.BackColor = System.Drawing.Color.White;
            this.shareButton.Image = global::LANSharing.Properties.Resources.if_send_01_1863866450;
            this.shareButton.Location = new System.Drawing.Point(501, 508);
            this.shareButton.Margin = new System.Windows.Forms.Padding(2);
            this.shareButton.MaximumSize = new System.Drawing.Size(200, 60);
            this.shareButton.MinimumSize = new System.Drawing.Size(100, 60);
            this.shareButton.Name = "shareButton";
            this.shareButton.Size = new System.Drawing.Size(100, 60);
            this.shareButton.TabIndex = 2;
            this.shareButton.UseVisualStyleBackColor = false;
            this.shareButton.Click += new System.EventHandler(this.shareButton_Click);
            this.shareButton.MouseEnter += new System.EventHandler(this.shareButton_MouseEnter);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleName = "cancelButton";
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelButton.AutoSize = true;
            this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelButton.BackColor = System.Drawing.Color.White;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.cancelButton.Image = global::LANSharing.Properties.Resources.cancel50;
            this.cancelButton.Location = new System.Drawing.Point(201, 508);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(0);
            this.cancelButton.MaximumSize = new System.Drawing.Size(200, 120);
            this.cancelButton.MinimumSize = new System.Drawing.Size(100, 60);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 60);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            this.cancelButton.MouseEnter += new System.EventHandler(this.cancelButton_MouseEnter);
            // 
            // state_offline_dot
            // 
            this.state_offline_dot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.state_offline_dot.AutoSize = true;
            this.state_offline_dot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.state_offline_dot.Image = global::LANSharing.Properties.Resources.metro_state_offline32;
            this.state_offline_dot.Location = new System.Drawing.Point(634, 89);
            this.state_offline_dot.MinimumSize = new System.Drawing.Size(30, 30);
            this.state_offline_dot.Name = "state_offline_dot";
            this.state_offline_dot.Size = new System.Drawing.Size(30, 30);
            this.state_offline_dot.TabIndex = 20;
            this.state_offline_dot.Click += new System.EventHandler(this.label1_Click);
            // 
            // state_online_dot
            // 
            this.state_online_dot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.state_online_dot.AutoSize = true;
            this.state_online_dot.Image = global::LANSharing.Properties.Resources.metro_state_online32;
            this.state_online_dot.Location = new System.Drawing.Point(634, 89);
            this.state_online_dot.MinimumSize = new System.Drawing.Size(30, 30);
            this.state_online_dot.Name = "state_online_dot";
            this.state_online_dot.Size = new System.Drawing.Size(30, 30);
            this.state_online_dot.TabIndex = 19;
            this.state_online_dot.Click += new System.EventHandler(this.state_online_dot_Click);
            // 
            // user_tile
            // 
            this.user_tile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.user_tile.AutoSize = true;
            this.user_tile.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.user_tile.Location = new System.Drawing.Point(678, 45);
            this.user_tile.Margin = new System.Windows.Forms.Padding(0);
            this.user_tile.MaximumSize = new System.Drawing.Size(160, 160);
            this.user_tile.MinimumSize = new System.Drawing.Size(80, 80);
            this.user_tile.Name = "user_tile";
            this.user_tile.Size = new System.Drawing.Size(80, 100);
            this.user_tile.Style = MetroFramework.MetroColorStyle.White;
            this.user_tile.TabIndex = 17;
            this.user_tile.TileImage = ((System.Drawing.Image)(resources.GetObject("user_tile.TileImage")));
            this.user_tile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.user_tile.UseTileImage = true;
            this.user_tile.Click += new System.EventHandler(this.user_tile_Click);
            this.user_tile.MouseEnter += new System.EventHandler(this.user_tile_MouseEnter);
            // 
            // browseFile
            // 
            this.browseFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseFile.AutoSize = true;
            this.browseFile.BackgroundImage = global::LANSharing.Properties.Resources.metro_file90;
            this.browseFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.browseFile.Location = new System.Drawing.Point(450, 183);
            this.browseFile.Margin = new System.Windows.Forms.Padding(0);
            this.browseFile.Name = "browseFile";
            this.browseFile.Size = new System.Drawing.Size(100, 100);
            this.browseFile.TabIndex = 16;
            this.browseFile.UseVisualStyleBackColor = true;
            this.browseFile.Click += new System.EventHandler(this.browseFile_Click);
            this.browseFile.MouseEnter += new System.EventHandler(this.browseFile_MouseEnter);
            // 
            // browseDirectory
            // 
            this.browseDirectory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseDirectory.AutoSize = true;
            this.browseDirectory.BackColor = System.Drawing.Color.Transparent;
            this.browseDirectory.BackgroundImage = global::LANSharing.Properties.Resources.metro_directory90;
            this.browseDirectory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.browseDirectory.Location = new System.Drawing.Point(250, 183);
            this.browseDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.browseDirectory.Name = "browseDirectory";
            this.browseDirectory.Size = new System.Drawing.Size(100, 100);
            this.browseDirectory.TabIndex = 14;
            this.browseDirectory.UseVisualStyleBackColor = false;
            this.browseDirectory.Click += new System.EventHandler(this.browseDirectory_Click);
            this.browseDirectory.MouseEnter += new System.EventHandler(this.browseDirectory_MouseEnter);
            // 
            // settingsButton
            // 
            this.settingsButton.AutoSize = true;
            this.settingsButton.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.settingsButton.Location = new System.Drawing.Point(120, 65);
            this.settingsButton.Margin = new System.Windows.Forms.Padding(2);
            this.settingsButton.MaximumSize = new System.Drawing.Size(100, 100);
            this.settingsButton.MinimumSize = new System.Drawing.Size(50, 50);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(50, 52);
            this.settingsButton.Style = MetroFramework.MetroColorStyle.White;
            this.settingsButton.TabIndex = 13;
            this.settingsButton.TileImage = global::LANSharing.Properties.Resources.metro_settings48;
            this.settingsButton.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.settingsButton.UseTileImage = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            this.settingsButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.settingsButton_Click);
            this.settingsButton.MouseEnter += new System.EventHandler(this.settingsButton_MouseEnter);
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = true;
            this.refreshButton.BackColor = System.Drawing.Color.White;
            this.refreshButton.Location = new System.Drawing.Point(50, 65);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(2);
            this.refreshButton.MaximumSize = new System.Drawing.Size(100, 100);
            this.refreshButton.MinimumSize = new System.Drawing.Size(50, 50);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(50, 50);
            this.refreshButton.Style = MetroFramework.MetroColorStyle.White;
            this.refreshButton.TabIndex = 4;
            this.refreshButton.TileImage = global::LANSharing.Properties.Resources.metro_refresh2;
            this.refreshButton.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.refreshButton.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.refreshButton.UseTileImage = true;
            this.refreshButton.Click += new System.EventHandler(this.refresh_Click);
            this.refreshButton.MouseEnter += new System.EventHandler(this.refreshButton_MouseEnter);
            // 
            // GUI
            // 
            this.AccessibleName = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shareButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.state_offline_dot);
            this.Controls.Add(this.state_online_dot);
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.user_tile);
            this.Controls.Add(this.browseFile);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.browseDirectory);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.description);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.refreshButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(1500, 900);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "GUI";
            this.Padding = new System.Windows.Forms.Padding(30, 60, 30, 30);
            this.ShadowType = MetroFramework.Forms.MetroForm.MetroFormShadowType.None;
            this.Style = MetroFramework.MetroColorStyle.Blue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.onlineIconContextMenu_Click);
            this.contextMenuTaskBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon taskbarIcon;
        private System.Windows.Forms.Button shareButton;
        private System.Windows.Forms.Button cancelButton;
        private MetroFramework.Controls.MetroLabel description;
        private System.Windows.Forms.ContextMenuStrip contextMenuTaskBar;
        private System.Windows.Forms.ToolStripMenuItem apriToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onlineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offlineMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private MetroFramework.Controls.MetroTile refreshButton;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        public MetroFramework.Controls.MetroLabel firstNameLabel;
        public MetroFramework.Controls.MetroLabel lastNameLabel;
        public MetroFramework.Controls.MetroTile settingsButton;
        private System.Windows.Forms.ToolStripMenuItem settingsStripMenuItem;
        private System.Windows.Forms.Button browseDirectory;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Button browseFile;
        public MetroFramework.Controls.MetroTile user_tile;
        public MetroFramework.Controls.MetroLabel stateLabel;
        private System.Windows.Forms.Label state_online_dot;
        private System.Windows.Forms.Label state_offline_dot;
        private System.Windows.Forms.ToolTip toolTipShareDirectory;
        private MetroFramework.Controls.MetroLabel label1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

