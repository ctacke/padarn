#region License
// Copyright ©2017 Tacke Consulting (dba OpenNETCF)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
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
