using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;

namespace RPCExampleClient
{
	class Helper
	{
		private const string SERVER_ADDRESS = "192.168.11.9";
		private const string SERVICE_URI_FORMAT = "http://{0}/RPCExample.aspx?{1}";
		private const string UNABLE_TO_CONNECT = "Unable to connect to the remote server";
		private const string BOOK_NOT_FOUND = "Book not found in library";

		private const string VERB_AUTHENTICATE = "Authenticate";
		private const string VERB_QUERY = "Query";

		private static string loginToken = String.Empty;
		private string searchResult = String.Empty;

		public static bool IsAuthenticated
		{
			get { return !String.IsNullOrEmpty(loginToken); }
		}

		/// <summary>
		/// Login method
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="userPassword">User password</param>
		/// <returns>True if succeeds</returns>
		public static bool CallRemoteAuthenticateMethod(string userName, string userPassword)
		{
			//1. Establish full URL with action verb
			string url = String.Format(SERVICE_URI_FORMAT, SERVER_ADDRESS, VERB_AUTHENTICATE);

			//2. Establish POST message body
			string contentString = String.Format("userName={0}&userPW={1}", 
				userName, userPassword);
			byte[] contentBytes = Encoding.ASCII.GetBytes(contentString);

			loginToken = RPCCallResult(true, url, contentBytes, "/authenticateResult");

			if (!String.IsNullOrEmpty(loginToken))
			{
				if (loginToken.Equals(UNABLE_TO_CONNECT))
				{
					throw new Exception(UNABLE_TO_CONNECT);
				}
			}

			return !String.IsNullOrEmpty(loginToken);
		}

		/// <summary>
		/// Method for invoking query on author, title, or ISBN
		/// </summary>
		/// <param name="title">Title of book</param>
		/// <param name="author">Author Name</param>
		/// <returns></returns>
		public static string CallRemoteQueryMethod(string title, string author)
		{
			//1. Establish full URL with action verb
			string url = String.Format(SERVICE_URI_FORMAT, SERVER_ADDRESS, VERB_QUERY);

			//2. Establish GET URL
			string urlFullString = 
				HttpUtility.UrlEncode(String.Format("token={0}&author={1}&title={2}",
				loginToken, author, title));

			//3. Run auxilary HttpWebRequest function
			string queryResult = RPCCallResult(false, url + "&" + urlFullString, null, 
				"/queryResult");

			if (!String.IsNullOrEmpty(queryResult) && queryResult.Equals(UNABLE_TO_CONNECT))
			{
				throw new Exception(UNABLE_TO_CONNECT);
			}

			//4. Return
			return queryResult;
		}

		private static string RPCCallResult(bool isPost, string url, byte[] contentBytes, string nodeName)
		{
			string retVal = String.Empty;

			//1. Establish web request object
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = isPost ? "POST" : "GET";
			request.ContentLength = contentBytes != null ? contentBytes.Length : 0;
			request.ContentType = "application/x-www-form-urlencoded";

			try
			{
				//2. Open Request and write message to it
				if (isPost)
				{
					using (Stream s = request.GetRequestStream())
					{
						s.Write(contentBytes, 0, contentBytes.Length);
					}
				}

				//3. Make sure valid response came back
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode != HttpStatusCode.OK)
				{
					response.Close();
					return String.Empty;
				}

				//4. Parse response - expecting a Guid back
				XmlDocument doc = new XmlDocument();
				string xml;
				using (StreamReader r = new StreamReader(response.GetResponseStream()))
				{
					xml = r.ReadToEnd();
				}

				if (!String.IsNullOrEmpty(xml))
				{
					//5. Read result text for our target. 
					doc.LoadXml(xml);
					try
					{
						XmlNode result = doc.SelectSingleNode(nodeName);
						retVal = result != null ? result.InnerText : String.Empty;
					}
					catch (System.Xml.XPath.XPathException xpe)
					{
						System.Windows.Forms.MessageBox.Show("Error in parsing XML: " + xpe.Message);
					}
				}
			}
			catch (WebException we)
			{
				retVal = UNABLE_TO_CONNECT;
			}

			return retVal;
		}
	}
}
