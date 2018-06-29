namespace RPCExampleClient
{
    partial class SearchForm
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
			this.DeviceTime = new System.Windows.Forms.Label();
			this.cmdSearch = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTitleSearch = new System.Windows.Forms.TextBox();
			this.txtAuthorSearch = new System.Windows.Forms.TextBox();
			this.lblAuthor = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lblISBN = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// DeviceTime
			// 
			this.DeviceTime.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.DeviceTime.AutoSize = true;
			this.DeviceTime.Location = new System.Drawing.Point(13, 105);
			this.DeviceTime.Name = "DeviceTime";
			this.DeviceTime.Size = new System.Drawing.Size(0, 13);
			this.DeviceTime.TabIndex = 7;
			// 
			// cmdSearch
			// 
			this.cmdSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.cmdSearch.Location = new System.Drawing.Point(84, 163);
			this.cmdSearch.Name = "cmdSearch";
			this.cmdSearch.Size = new System.Drawing.Size(296, 23);
			this.cmdSearch.TabIndex = 3;
			this.cmdSearch.Text = "Search!";
			this.cmdSearch.UseVisualStyleBackColor = true;
			this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(14, 15);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 82);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search Fields";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Title";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(28, 69);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Author";
			// 
			// txtTitleSearch
			// 
			this.txtTitleSearch.Location = new System.Drawing.Point(84, 36);
			this.txtTitleSearch.MaxLength = 30;
			this.txtTitleSearch.Name = "txtTitleSearch";
			this.txtTitleSearch.Size = new System.Drawing.Size(124, 20);
			this.txtTitleSearch.TabIndex = 1;
			// 
			// txtAuthorSearch
			// 
			this.txtAuthorSearch.Location = new System.Drawing.Point(85, 66);
			this.txtAuthorSearch.MaxLength = 30;
			this.txtAuthorSearch.Name = "txtAuthorSearch";
			this.txtAuthorSearch.Size = new System.Drawing.Size(124, 20);
			this.txtAuthorSearch.TabIndex = 2;
			// 
			// lblAuthor
			// 
			this.lblAuthor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblAuthor.Location = new System.Drawing.Point(301, 66);
			this.lblAuthor.Name = "lblAuthor";
			this.lblAuthor.Size = new System.Drawing.Size(124, 20);
			this.lblAuthor.TabIndex = 23;
			// 
			// lblTitle
			// 
			this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTitle.Location = new System.Drawing.Point(300, 36);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(124, 20);
			this.lblTitle.TabIndex = 22;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(244, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 21;
			this.label3.Text = "Author";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(244, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "Title";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(230, 15);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 122);
			this.groupBox2.TabIndex = 19;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Search Results";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(229, 105);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(0, 13);
			this.label5.TabIndex = 18;
			// 
			// lblISBN
			// 
			this.lblISBN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblISBN.Location = new System.Drawing.Point(302, 95);
			this.lblISBN.Name = "lblISBN";
			this.lblISBN.Size = new System.Drawing.Size(124, 20);
			this.lblISBN.TabIndex = 25;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(245, 98);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 13);
			this.label6.TabIndex = 24;
			this.label6.Text = "ISBN";
			// 
			// SearchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 210);
			this.Controls.Add(this.lblISBN);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lblAuthor);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtAuthorSearch);
			this.Controls.Add(this.txtTitleSearch);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdSearch);
			this.Controls.Add(this.DeviceTime);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SearchForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "RPC Service Client Sample";
			this.Activated += new System.EventHandler(this.SearchForm_Activated);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label DeviceTime;
		private System.Windows.Forms.Button cmdSearch;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTitleSearch;
		private System.Windows.Forms.TextBox txtAuthorSearch;
		private System.Windows.Forms.Label lblAuthor;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblISBN;
		private System.Windows.Forms.Label label6;
    }
}

