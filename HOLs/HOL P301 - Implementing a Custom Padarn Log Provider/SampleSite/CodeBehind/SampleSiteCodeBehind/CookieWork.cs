using System;
using System.IO;
using System.Text;
using System.Xml;
using OpenNETCF.Web.UI;
using System.Collections.Generic;
using System.Net;
using OpenNETCF.Web.Html;

namespace SampleSite
{
	public class CookieWork : Page
	{
		#region Constants
		private const string VERB_FUNCTION = "function=";
		private const string VERB_MESSAGE = "message=";
		private const string CLASS_NAME = "AUTHENTICATION";

		private const string COOKIENAME = "userInfo";
		private const string COOKIEVAL_USER_NAME = "userName";
		private const string COOKIEVAL_GUID = "GUID";
		private const string COOKIEVAL_LAST_VISIT = "lastVisit";

		#endregion



		protected override void Page_Load(object sender, EventArgs e)
		{
			//string s = null;
			//s.ToString();

			if (Request.Form.Count > 0)
			{
				if (Request.Form["Submit"] != null)
				{
					string UID = Request.Form["UserName"];
					string PW = Request.Form["PW"];

					Guid uoGuid = Authentication.ValidateUser(UID, PW);
					if (uoGuid != Guid.Empty)
					{
						//1. Set GUID in Cookie
						OpenNETCF.Web.HttpCookie myCookie = new OpenNETCF.Web.HttpCookie(COOKIENAME);
						myCookie.Values[COOKIEVAL_USER_NAME] = UID;
						myCookie.Values[COOKIEVAL_GUID] = uoGuid.ToString();
						myCookie.Values[COOKIEVAL_LAST_VISIT] = DateTime.Now.ToString();
						myCookie.Expires = DateTime.Now.AddDays(1);

						if (Request.Browser.Cookies)
						{
							//2. Verify browser can accept cookies
							Response.SetCookie(myCookie);
						}

						//3. Do redirect to authenticated page
						Response.Redirect("CookieWork.aspx");
					}
					else
					{
						//Show bad credentials screen
						DisplayLoginForm(true);
					}

				}
				else if (Request.Form["LogOut"] != null)
				{
					//1. Perform log-out
					string UID = Request.Form["LogOut"];
					Authentication.LogOff(UID);

					//2. Delete cookie
					if (Request.Cookies[COOKIENAME] != null)
					{
						OpenNETCF.Web.HttpCookie myCookie = Request.Cookies[COOKIENAME];
						myCookie.Expires = DateTime.Now.AddDays(-1);
						if (Request.Browser.Cookies)
						{
							//Verify browser can accept cookies
							Response.SetCookie(myCookie);
						}
					}

					//3. Refresh page
					Response.Redirect("CookieWork.aspx");
				}
			}
			else
			{
				//Check GUID in cookie; redirect to authenticated page if so
				if (Request.Cookies[COOKIENAME] != null && Request.Cookies[COOKIENAME].HasKeys)
				{
					string guidCookie = Request.Cookies[COOKIENAME].Values[COOKIEVAL_GUID];
					Guid g = new Guid(guidCookie);
					Authentication.UserObject uo;

					if (!(uo = Authentication.ValidateUser(g)).Null && !uo.IsGuidExpired())
					{
						if (Request.Cookies[COOKIENAME][COOKIEVAL_LAST_VISIT] == null)
						{
							//We got here upon manual passage of credentials
							DisplayAuthenticated(false, uo.UserName);
						}
						else
						{
							//We got here by reading cookie
							DisplayAuthenticated(true, uo.UserName);
						}
					}
					else
					{
						//We need to re-authenticate because GUID is invalid or expired
						DisplayLoginForm(false);
					}
				}
				else
				{
					//We need to re-authenticate; no cookie was found
					DisplayLoginForm(false);
				}
			}
		}

		private void DisplayAuthenticated(bool viaCookie, string UID)
		{
			//This will display to the user a few pieces of information about the session data stored
			Document doc = new Document();
			doc.Head = new DocumentHead("OpenNETCF Padarn Web Server", new StyleInfo("css/SampleSite.css"));

			Utility.AddPadarnHeaderToDocument(doc, true, "Execute");

			Div containerDiv = new Div("container");
			Form form = new Form("CookieWork.aspx", FormMethod.Post);

			if (viaCookie)
			{
				form.Add(new RawText(String.Format("Your are authenticated via cookie")));
			}
			else
			{
				form.Add(new RawText(String.Format("Your are authenticated via normal credentials")));
			}
			form.Add(new LineBreak());
			form.Add(new RawText(String.Format("Cookie last visit on {0}", Request.Cookies[COOKIENAME][COOKIEVAL_LAST_VISIT] == null ?
				"NEVER" : DateTime.Parse(Request.Cookies[COOKIENAME][COOKIEVAL_LAST_VISIT]).ToString())));
			form.Add(new LineBreak());
			form.Add(new RawText(String.Format("Cookie GUID is {0}", Request.Cookies[COOKIENAME][COOKIEVAL_GUID])));

			//Let the user log out at any point
			form.Add(new LineBreak());
			form.Add(new LineBreak());
			form.Add(new Button(new ButtonInfo(ButtonType.Submit, "Log Out")));
			form.Add(new Hidden("LogOut", UID));

			containerDiv.Add(form);
			doc.Body.Add(containerDiv);

			if (Request.Cookies.Count > 0 && Request.Cookies[COOKIENAME] != null)
			{
				//Modify cookie to show we've been here before
				OpenNETCF.Web.HttpCookie myCookie = Request.Cookies[0];
				myCookie.Values[COOKIEVAL_LAST_VISIT] = DateTime.Now.ToString();
				Response.SetCookie(myCookie);
			}

			//Write out the response
			Response.Write(doc.OuterHtml);
			Response.Flush();
		}

		private void DisplayLoginForm(bool badPW)
		{
			Document doc = new Document();
			doc.Head = new DocumentHead("OpenNETCF Padarn Web Server", new StyleInfo("css/SampleSite.css"));

			Utility.AddPadarnHeaderToDocument(doc, true, "Execute");

			Div containerDiv = new Div("container");

			//We need to prompt user for credentials
			Div div = new Div("UserName");
			Form form = new Form("CookieWork.aspx", FormMethod.Post);

			if (badPW)
			{
				form.Add(new RawText(string.Format("<label>{0}</label>", "! Invalid Credentials !")));
				form.Add(new LineBreak());
				form.Add(new LineBreak());
			}

			form.Add(new RawText(string.Format("<label>{0}</label>", "User Name ")));
			form.Add(new Input("UserName"));

			form.Add(new LineBreak());

			//Create a standard password input box
			form.Add(new RawText(string.Format("<label>{0}</label>", "Password ")));
			form.Add(new RawText("<input name=\"PW\" type=\"password\">"));

			form.Add(new LineBreak());
			form.Add(new LineBreak());

			//Create a submit and reset button
			form.Add(new Button(new ButtonInfo(ButtonType.Submit, "Submit")));
			form.Add(new Hidden("Submit", "UserName"));

			form.Add(new Button(new ButtonInfo(ButtonType.Reset, "Reset")));

			div.Add(form);
			containerDiv.Add(div);
			doc.Body.Add(containerDiv);

			//Write out the response
			Response.Write(doc.OuterHtml);
			Response.Flush();
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

