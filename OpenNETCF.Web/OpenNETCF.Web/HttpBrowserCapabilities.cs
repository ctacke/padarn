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
