using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Generic;

namespace RPCExampleClient
{
    public partial class SearchForm : Form
    {
		public SearchForm()
        {
            InitializeComponent();
			this.Enabled = false;
        }

		private void cmdSearch_Click(object sender, EventArgs e)
		{
			Cursor current = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			string resultValue = Helper.CallRemoteQueryMethod(txtTitleSearch.Text, txtAuthorSearch.Text);
			Cursor.Current = current;

			ParseSearchResult(resultValue);
		}

		private void SearchForm_Activated(object sender, EventArgs e)
		{
			if (Helper.IsAuthenticated)
			{
				this.Enabled = true;
			}
			else
			{
				LoginForm loginInstance = new LoginForm();
				loginInstance.StartPosition = FormStartPosition.CenterScreen;
				loginInstance.Show();
				loginInstance.BringToFront();
			}
		}

		/// <summary>
		/// Method that assists in breaking apart the RPC web service result when book is found
		/// </summary>
		/// <param name="resultValue"></param>
		private void ParseSearchResult(string resultValue)
		{
			if (String.IsNullOrEmpty(resultValue))
			{
				lblAuthor.Text = lblISBN.Text = lblTitle.Text = String.Empty;
				System.Windows.Forms.MessageBox.Show("Book not found");
			}
			else
			{
				//Individual parameters will be separated by semi-colon
				string corrected = resultValue.Replace("\r\n", ";");
				string[] chunkedValues = resultValue.Split(';');
				Dictionary<string, string> parsedValues = new Dictionary<string, string>();

				foreach (string qualifier in chunkedValues)
				{
					string[] pair = qualifier.Split('=');
					if (pair.Length == 2)
					{
						//With this code, we only consider the first match
						if (!parsedValues.ContainsKey(pair[0]))
							parsedValues.Add(pair[0], pair[1]);
					}
				}

				lblTitle.Text = parsedValues.ContainsKey("Title") ? parsedValues["Title"] : String.Empty;
				lblAuthor.Text = parsedValues.ContainsKey("Author") ? parsedValues["Author"] : String.Empty;
				lblISBN.Text = parsedValues.ContainsKey("ISBN") ? parsedValues["ISBN"] : String.Empty;
			}
		}
    }
}
