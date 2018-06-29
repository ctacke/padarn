using System;
using System.IO;
using System.Text;
using System.Xml;
using OpenNETCF.Web.UI;
using System.Collections.Generic;
using System.Net;
using OpenNETCF.Web.Html;
using OpenNETCF.Web.Html.Meta;

namespace SampleSite
{
	public class FormExample : Page
	{
		#region Constants

		private const string VERB_FUNCTION = "function=";
		private const string RADIO_CLASS = "RADIO_CLASS";
		private const string TEXT_INPUT = "TEXT_INPUT";
		private const string RADIO_INPUT = "RADIO_INPUT";

		private const string ACTION = "action";

		#endregion

		#region Page Load

		protected override void Page_Load(object sender, EventArgs e)
		{
			if (!FormHasData && !URIHasData)
			{
				CreateHTMLInput();
			}

			else if (!FormHasData && URIHasData)
			{
				/* (GET) User is passing in variable as encoded URI - use RawQueryString */

				// 1. Create a variable-length array of verbs and identifiers
				string incomingParameters = Request.RawQueryString.ToLower();
				string[] actionList = incomingParameters.Split('&');

				if (actionList.Length >= 1)
				{

					// 2. Switch on function
					string functionName = actionList[0];

					switch (functionName)
					{
						case "submit":
							{
								// 3. Use OpenNETCF HttpUtility to strip out HTML characters
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

								// Note: Format of URL should be 'action=<ACTION SPECIFIER>'
								string action = parsedValues.ContainsKey(ACTION) ? parsedValues[ACTION] : String.Empty;

								// 4. Redirect
								Response.Redirect("RedirectExample.aspx");

								break;
							}

						default:
							break;
					}
				}
			}

			else if (FormHasData && IsHttpPost)
			{
				/* (POST) User is passing in input through browser form - use Form object */

				// 1. Pull from Form
				string textInput = Request.Form[TEXT_INPUT] as string;
				string rdoVal = Request.Form[RADIO_INPUT] as string;
				string checkSelected = Request.Form["CheckSelected"] as string;

				// 2. Perform some server-side validation and processing on inputs 

				// 3. Display results to user
				CreateHTML(textInput, rdoVal, checkSelected);
			}
		}

		#endregion

		#region HTML Helpers

		/// <summary>
		/// Method that creates an HTML page with text input, radio group, checkbox, and submit button
		/// </summary>
		private void CreateHTMLInput()
		{
			//1. Create a simple input form with textbox, radio buttons, and sumbit button
			Document page = new Document();
			page.Head = new DocumentHead("OpenNETCF Padarn Web Server - Form Test Input", new StyleInfo("css/SampleSite.css"));
			page.Head.MetaTags.Add(new ContentType(CharSet.UTF8));

			Utility.AddPadarnHeaderToDocument(page, true, "Default");

			//2. Create a form object and set the method to POST
			Form form = new Form("FormExample.aspx", FormMethod.Post);

			//3.  Add text input and buttons
			form.Add(new Input(TEXT_INPUT));
			form.Add(new Button(new ButtonInfo(ButtonType.Submit, "Submit")));
			form.Add(new Button(new ButtonInfo(ButtonType.Reset, "Reset")));

			form.Add(new LineBreak());

			//4. Add RadioButtons
			RadioGroup rg = new RadioGroup(RADIO_INPUT);
			rg.Items.Add(new RadioItem("Value 1", "Val1", true, RADIO_CLASS));
			rg.Items.Add(new RadioItem("Value 2", "Val2", false, RADIO_CLASS));
			form.Add(rg);

			form.Add(new LineBreak());

			//5. Add checkboxes
			form.Add(new RawText("Choice selected?:" +
				"<input type=\"checkbox\" name=\"CheckSelected\" value=\"Selected\"" +
				"/>" +
				"<br />"));

			//6. Complete writing page
			page.Body.Elements.Add(form);
			Response.Write(page.OuterHtml);
			Response.Flush();
		}

		/// <summary>
		/// Function that outputs to user the one-line result of a URI request
		/// </summary>
		/// <param name="result">Value of URI action</param>
		private void CreateHTML(string result)
		{
			// 1. Create the document
			Document page = new Document();

			// 2. Add a header to the document
			page.Head = new DocumentHead("OpenNETCF Padarn Web Server - Form Parsing Result", new StyleInfo("css/SampleSite.css"));

			Utility.AddPadarnHeaderToDocument(page, true, "Default");

			// 3. Begin formatting
			Paragraph paragraph = new Paragraph(new FormattedText("Output", TextFormat.Bold, "4"), Align.Center);

			FormattedText ftResult = new FormattedText(String.Format("Result: {0}", 
				!String.IsNullOrEmpty(result) ? result : "NOTHING"), TextFormat.None, "4");

			// 4. Add our results
			paragraph.Elements.Add(new LineBreak());
			paragraph.Elements.Add(ftResult);
			page.Body.Elements.Add(paragraph);

			// 5. Send result back to user
			Response.Write(page.OuterHtml);
			Response.Flush();
		}

		/// <summary>
		/// Function that outputs to user the values of all input selections
		/// </summary>
		/// <param name="textInput">Value of text field</param>
		/// <param name="rdoVal">Value of radio group</param>
		/// <param name="checkSelected">Value of checkbox's checked state</param>
		private void CreateHTML(string textInput, string rdoVal, string checkSelected)
		{
			// 1. Create the document
			Document page = new Document();

			// 2. Add a header to the document
			page.Head = new DocumentHead("OpenNETCF Padarn Web Server - Form Parsing Result", new StyleInfo("css/SampleSite.css"));

			Utility.AddPadarnHeaderToDocument(page, true, "Default");

			// 3. Begin formatting
			Paragraph paragraph = new Paragraph(new FormattedText("Output", TextFormat.Bold, "4"), Align.Center);

			FormattedText ftTextInput = new FormattedText(String.Format("User inputted: {0}", textInput), TextFormat.None, "4");
			FormattedText ftRdoVal = new FormattedText(String.Format("Radio Selected: {0}", rdoVal), TextFormat.None, "4");
			FormattedText ftCheckSelected = new FormattedText(String.Format("Check selected: {0}", 
				(!String.IsNullOrEmpty(checkSelected) ? "TRUE" : "FALSE")), TextFormat.None, "4");

			// 4. Add our results
			paragraph.Elements.Add(new LineBreak());
			paragraph.Elements.Add(ftTextInput);
			paragraph.Elements.Add(new LineBreak());
			paragraph.Elements.Add(ftRdoVal);
			paragraph.Elements.Add(new LineBreak());
			paragraph.Elements.Add(ftCheckSelected);
			paragraph.Elements.Add(new LineBreak());

			// 5. Send result back to user
			page.Body.Elements.Add(paragraph);
			Response.Write(page.OuterHtml);
			Response.Flush();
		}

		#endregion

		#region Simple Properties

		private bool IsHttpPost
		{
			get { return (Request.RequestType == "POST"); }
		}

		private bool URIHasData
		{
			get { return !String.IsNullOrEmpty(Request.RawQueryString); }
		}

		private bool FormHasData
		{
			get { return Request.Form.HasKeys(); }
		}

		#endregion
	}
}

