using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RPCExampleClient
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(txtUserID.Text) && !String.IsNullOrEmpty(txtPassword.Text))
			{
				Cursor current = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					if (!Helper.CallRemoteAuthenticateMethod(txtUserID.Text, txtPassword.Text))
					{
						Cursor.Current = current;
						System.Windows.Forms.MessageBox.Show("Invalid credentials");
						txtUserID.Text = txtPassword.Text = String.Empty;
					}
					else
					{
						Cursor.Current = current;
						this.Close();
					}
				}
				catch (Exception exception)
				{
					System.Windows.Forms.MessageBox.Show(exception.Message);
				}
				finally
				{
					Cursor.Current = current;
				}
			}
			else
			{
				System.Windows.Forms.MessageBox.Show("Fill out user name and password");
			}
		}
	}
}
