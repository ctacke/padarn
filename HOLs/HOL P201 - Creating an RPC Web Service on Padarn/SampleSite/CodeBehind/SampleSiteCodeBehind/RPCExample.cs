using System;
using System.IO;
using System.Text;
using System.Xml;
using OpenNETCF.Web.UI;
using System.Collections.Generic;
using System.Net;

namespace SampleSite
{
	public class RPCExample : Page
	{
		#region Constants
		private const string VERB_FUNCTION = "function=";
		private const string VERB_MESSAGE = "message=";
		private static Library myLibrary = new Library();
		#endregion

		protected override void Page_Load(object sender, EventArgs e)
		{
			//1. Read incoming message body
			string incomingParameters = Request.RawQueryString.ToLower();

			//2. Create a variable-length array of verbs and identifiers
			string[] actionList = incomingParameters.Split('&');

			if (actionList.Length >= 1)
			{
				if (!String.IsNullOrEmpty(actionList[0]))
				{
					//3. Switch on function
					string functionName = actionList[0].Replace(VERB_FUNCTION, String.Empty);

					switch (functionName)
					{
						case "messagebox":
							{
								if (actionList.Length > 1 && !String.IsNullOrEmpty(actionList[1]) && actionList[1].LastIndexOf(VERB_MESSAGE) != -1)
									System.Windows.Forms.MessageBox.Show(actionList[1].Replace(VERB_MESSAGE, String.Empty));
								WriteResult("MessageBox", "Completed");
							}
							break;

						case "authenticate":
							{
								//Only allow HTTP POST for this method.
								if (!this.IsHttpPost)
								{
									WriteServiceError("authenticate", "Request method not supported. Use HTTP POST.");
									return;
								}

								//Format of POST content should be 'userName=XYZ&userPW=ABC'
								string userName = Request.Form["userName"];
								string userPW = Request.Form["userPW"];

								string GUID = myLibrary.Login(userName, userPW);

								//Format of POST content should be 'userName=ABC&userPW=DEF'
								WriteResult("authenticate", GUID);
							}
							break;

						case "query":
							{
								//Use OpenNETCF HttpUtility to strip out HTML characters
								string[] chunkedValues = OpenNETCF.Web.HttpUtility.UrlDecode(Request.RawQueryString).Split('&');
								Dictionary<string, string> parsedValues = new Dictionary<string, string>();

								foreach (string qualifier in chunkedValues)
								{
									string[] pair = qualifier.Split('=');
									if (pair.Length == 2)
									{
										parsedValues.Add(pair[0], pair[1]);
									}
								}

								//Format of URL should be 'token=123&title=ABC&author=DEF&ISBN=GHI'
								string token = parsedValues.ContainsKey("token") ? parsedValues["token"] : String.Empty;
								string title = parsedValues.ContainsKey("title") ? parsedValues["title"] : String.Empty;
								string author = parsedValues.ContainsKey("author") ? parsedValues["author"] : String.Empty;
								string ISBN = parsedValues.ContainsKey("ISBN") ? parsedValues["ISBN"] : String.Empty;

								if (myLibrary.IsValidAuthenticationToken(token))
								{
									//Only run query if valid token
									string queryResult = myLibrary.RetrieveBookProperties(title, author, ISBN);
									WriteResult("query", queryResult);
								}
								else
								{
									WriteResult("query", "Error");
								}
							}
							break;
					}
				}
			}

		}

		#region Service Methods

		/// <summary>
		/// Adds two integers.
		/// </summary>
		/// <param name="a">The first integer.</param>
		/// <param name="b">The second integer.</param>
		/// <returns>The sum of the specified integers.</returns>
		public int Add(int a, int b)
		{
			return (a + b);
		}

		/// <summary>
		/// Gets the current time of the device.
		/// </summary>
		/// <returns>The current time of the device.</returns>
		public string GetDeviceTime()
		{
			return DateTime.Now.ToString("u");
		}

		#endregion

		private bool IsHttpPost
		{
			get { return (Request.RequestType == "POST"); }
		}

		#region XML Helper Methods

		private void WriteServiceError(string serviceMethod, string message)
		{
			string errorElementName = String.Format("{0}Error", serviceMethod);

			this.WriteSingleXmlNode(errorElementName, message);
		}

		private void WriteResult(string serviceMethod, object result)
		{
			string resultElementName = String.Format("{0}Result", serviceMethod);

			this.WriteSingleXmlNode(resultElementName, result);
		}

		private void WriteSingleXmlNode(string elementName, object value)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (XmlTextWriter w = new XmlTextWriter(ms, Encoding.ASCII))
				{
					w.Formatting = Formatting.Indented;

					w.WriteStartDocument();

					w.WriteStartElement(elementName);
					w.WriteValue(value);
					w.WriteEndElement();

					w.WriteEndDocument();
				}

				Response.BinaryWrite(ms.GetBuffer());
				Response.Flush();
			}
		}

		#endregion
	}
}

