using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web
{
    /// <summary>
    /// Http methods
    /// </summary>
    [Flags]
    public enum HttpMethod
    {
        Unknown = 0,
        GET = 1,
        PUT = GET * 2,
        HEAD = PUT * 2,
        POST = HEAD * 2,
        DEBUG = POST * 2,
        DELETE = DEBUG * 2,
        ANY = GET + PUT + HEAD + POST + DEBUG + DELETE
    }
}
