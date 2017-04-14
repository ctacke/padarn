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

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web.Logging
{
  /// <summary>
  /// Contains data about a padarn page access
  /// </summary>
  public sealed class LogDataItem
  {
    internal LogDataItem(NameValueCollection headers, string page, string clientIP, ServerConfig config)
    {
      Headers = headers;
      PageName = page;
      RemoteClientIP = clientIP;
      ServerConfiguration = config;
    }

    /// <summary>
    /// HTTP Headers passed into the page request
    /// </summary>
    public NameValueCollection Headers { get; set; }
    /// <summary>
    /// Page being called
    /// </summary>
    public string PageName { get; set; }
    /// <summary>
    /// IP address of the client making the page request
    /// </summary>
    public string RemoteClientIP { get; set; }
    /// <summary>
    /// Current Padarn configuration settings
    /// </summary>
    public ServerConfig ServerConfiguration { get; set; }
  }
}
