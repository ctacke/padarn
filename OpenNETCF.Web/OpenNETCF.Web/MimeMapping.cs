namespace OpenNETCF.Web
{
    using System;
    using System.Collections;

    internal class MimeMapping
    {
        private static readonly Hashtable extensionToMimeMappingTable = new Hashtable(190, StringComparer.CurrentCultureIgnoreCase);

        static MimeMapping()
        {
            AddMimeMapping(".*", "text/plain");

            AddMimeMapping(".bmp", "image/bmp");
            AddMimeMapping(".gif", "image/gif");
            AddMimeMapping(".ico", "image/x-icon");
            AddMimeMapping(".jpg", "image/jpeg");
            AddMimeMapping(".tif", "image/tiff");
            AddMimeMapping(".tiff", "image/tiff");
            AddMimeMapping(".png", "image/png");
            
            AddMimeMapping(".html", "text/html");
            AddMimeMapping(".htm", "text/html");
            AddMimeMapping(".js", "application/x-javascript");
            AddMimeMapping(".vb", "text/vbscript");
            AddMimeMapping(".txt", "text/plain");
            AddMimeMapping(".xml", "text/xml");
            AddMimeMapping(".aspx", "text/html");
            AddMimeMapping(".css", "text/css");
            
            AddMimeMapping(".pdf", "application/pdf");
            AddMimeMapping(".zip", "application/x-zip-compressed");
        }

        private MimeMapping()
        {
        }

        private static void AddMimeMapping(string extension, string mimeType)
        {
            extensionToMimeMappingTable.Add(extension, mimeType);
        }

        internal static string GetMimeMapping(string fileName)
        {
            string text = null;

            var startIndex = -1;
            if (fileName != null)
            {
                startIndex = fileName.LastIndexOf('.');
            }

            if ((0 < startIndex) && (startIndex > fileName.LastIndexOf(System.IO.Path.DirectorySeparatorChar)))
            {
                text = (string)extensionToMimeMappingTable[fileName.Substring(startIndex)];
            }
            if (text == null)
            {
                text = (string)extensionToMimeMappingTable[".*"];
            }
            return text;
        }
    }
}
