namespace SampleSite
{
    partial class ControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.padarnIP = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.miOpen = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.miStartPadarn = new System.Windows.Forms.MenuItem();
            this.miStop = new System.Windows.Forms.MenuItem();
            this.deviceIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(3, 93);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 32);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(99, 93);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(91, 32);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // padarnIP
            // 
            this.padarnIP.Location = new System.Drawing.Point(3, 11);
            this.padarnIP.Name = "padarnIP";
            this.padarnIP.Size = new System.Drawing.Size(232, 20);
            this.padarnIP.Text = "Current Server IP:";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(3, 61);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(232, 20);
            this.lblStatus.Text = "Padarn is currently Stopped";
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.miOpen);
            this.contextMenu1.MenuItems.Add(this.menuItem2);
            this.contextMenu1.MenuItems.Add(this.miStartPadarn);
            this.contextMenu1.MenuItems.Add(this.miStop);
            // 
            // miOpen
            // 
            this.miOpen.Text = "Open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "-";
            // 
            // miStartPadarn
            // 
            this.miStartPadarn.Text = "Start Padarn";
            this.miStartPadarn.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // miStop
            // 
            this.miStop.Enabled = false;
            this.miStop.Text = "Stop Padarn";
            this.miStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // deviceIP
            // 
            this.deviceIP.Location = new System.Drawing.Point(3, 31);
            this.deviceIP.Name = "deviceIP";
            this.deviceIP.Size = new System.Drawing.Size(232, 20);
            this.deviceIP.Text = "Current Server IP:";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(238, 215);
            this.Controls.Add(this.deviceIP);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.padarnIP);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControlForm";
            this.Text = "Padarn Web Server Manager";
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label padarnIP;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem miOpen;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem miStartPadarn;
        private System.Windows.Forms.MenuItem miStop;
        private System.Windows.Forms.Label deviceIP;
    }
}