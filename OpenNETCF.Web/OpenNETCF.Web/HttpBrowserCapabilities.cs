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
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Enables the server to gather information on the capabilities of the browser that is running on the client.
    /// </summary>
    public class HttpBrowserCapabilities : HttpCapabilitiesBase
    {
        internal HttpBrowserCapabilities(NameValueCollection headers)
            : base(headers)
        {


        }

        private string[] GetProducts(string userAgentString)
        {
            List<string> products = new List<string>();
            string pattern = @"\s*(?<ua>[^()<>@,;:\\/\]\[?={} ]+)(\/(?<ver>[^()<>@,;:\\/\]\[?={} ]+))?(\s+\[.*\])?(\s+\(((?'detail'[^;)]+)([; ]*))+\))?";

            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(userAgentString))
            {
                Debug.WriteLine(string.Format("User-Agent: {0}", match.Groups["ua"]));
                Debug.WriteLine(string.Format("Version: {0}", match.Groups["ver"]));
                Debug.WriteLine("Compatible: ");
                foreach (Capture cap in match.Groups["detail"].Captures)
                    Debug.WriteLine(string.Format("\t{0}", cap));
            }

            return products.ToArray();
        }
    }
}
