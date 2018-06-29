using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;
using System.IO;
using System.Runtime.InteropServices;

namespace SampleSite
{
	public class FileReceiver : Page
	{
		public const string VIRTUAL_DIRECTORY_LOCAL = @"\Windows\DumpSite\";

		protected override void Page_Load(object sender, EventArgs e)
		{
			Document doc = new Document(new DocumentHead("OpenNETCF File Upload Example"));

			if (Request.HttpMethod == "POST")
			{
				// We will save this file to the Virtual Directory folder
				if (this.Request.ContentLength > 0 && this.Request.Files.Count > 0 && this.Request.Files[0] != null)
				{
					try
					{
						//Request FileName includes full path from client
						string remoteName = Path.GetFileName(this.Request.Files[0].FileName);
						string fullPath = VIRTUAL_DIRECTORY_LOCAL + remoteName;

						int uploadSize = this.Request.ContentLength;

						//We'll need to create the directory if it doesn't exist
						if (!Directory.Exists(VIRTUAL_DIRECTORY_LOCAL))
						{
							Directory.CreateDirectory(VIRTUAL_DIRECTORY_LOCAL);
						}
						else if (File.Exists(fullPath))
						{
							//Overwrite file
							File.Delete(fullPath);
						}

						//Verify there's enough disk space
						DiskFreeSpace folderSpace = new DiskFreeSpace();

						//Use our P/Invoke below to determine free space
						if (!GetDiskFreeSpaceEx(VIRTUAL_DIRECTORY_LOCAL, ref folderSpace.FreeBytesAvailable, 
							ref folderSpace.TotalBytes, ref folderSpace.TotalFreeBytes))
						{
							throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving free disk space");
						}
						else if (uploadSize > folderSpace.FreeBytesAvailable)
						{
							doc.Body.Elements.Add(new RawText("Insufficient disk space on server"));
						}
						else
						{
							//Save file locally
							this.Request.Files[0].SaveAs(fullPath);

							FileInfo fiUploaded = new FileInfo(fullPath);

							//Indicate success to user
							doc.Body.Elements.Add(new RawText("Upload success"));
							doc.Body.Elements.Add(new LineBreak());
							doc.Body.Elements.Add(new RawText(String.Format("File upload completed at {0}", 
								fiUploaded.CreationTime.ToLongTimeString())));
						}
					}
					catch
					{
						//Indicate failure to user
						doc.Body.Elements.Add(new RawText("Upload failure"));
					}
				}
			}
			else
			{
				//Utilitarian browse/upload form
				Form form = new Form("FileReceiver.aspx", FormMethod.Post, "upload", "upload");
				form.ContentType = "multipart/form-data";
				form.Add(new RawText("File to upload:"));
				form.Add(new Upload("upfile"));
				form.Add(new Button(new ButtonInfo(ButtonType.Submit, "Upload")));
				doc.Body.Elements.Add(form);
			}

			//Send the document html to the Response object
			Response.Write(doc.OuterHtml);
			//Flush the response
			Response.Flush();
		}

		#region Free Space Helpers

		private struct DiskFreeSpace
		{
			/// <summary> 
			/// The total number of free bytes on the disk that are available
			/// </summary>
			public long FreeBytesAvailable;
			/// <summary>
			/// The total number of bytes on the disk that are available
			/// </summary>
			public long TotalBytes;
			/// <summary>
			/// The total number of free bytes on the disk.
			/// </summary>
			public long TotalFreeBytes;
		}

		[DllImport("coredll", SetLastError = true)]
		private static extern bool GetDiskFreeSpaceEx(string directoryName, ref long
			freeBytesAvailable, ref long totalBytes, ref long totalFreeBytes);

		#endregion

	}
}
