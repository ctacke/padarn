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
