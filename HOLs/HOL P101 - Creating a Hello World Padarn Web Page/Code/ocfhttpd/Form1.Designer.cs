namespace ocfhttpd
{
  partial class Form1
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
      this.label1 = new System.Windows.Forms.Label();
      this.ipLabel = new System.Windows.Forms.Label();
      this.startButton = new System.Windows.Forms.Button();
      this.stopButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(3, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 20);
      this.label1.Text = "Server IP:";
      // 
      // ipLabel
      // 
      this.ipLabel.Location = new System.Drawing.Point(82, 23);
      this.ipLabel.Name = "ipLabel";
      this.ipLabel.Size = new System.Drawing.Size(149, 20);
      this.ipLabel.Text = "<unknown>";
      // 
      // startButton
      // 
      this.startButton.Location = new System.Drawing.Point(4, 69);
      this.startButton.Name = "startButton";
      this.startButton.Size = new System.Drawing.Size(84, 31);
      this.startButton.TabIndex = 2;
      this.startButton.Text = "Start";
      // 
      // stopButton
      // 
      this.stopButton.Location = new System.Drawing.Point(94, 69);
      this.stopButton.Name = "stopButton";
      this.stopButton.Size = new System.Drawing.Size(84, 31);
      this.stopButton.TabIndex = 3;
      this.stopButton.Text = "Stop";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(234, 153);
      this.Controls.Add(this.stopButton);
      this.Controls.Add(this.startButton);
      this.Controls.Add(this.ipLabel);
      this.Controls.Add(this.label1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label ipLabel;
    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Button stopButton;
  }
}

