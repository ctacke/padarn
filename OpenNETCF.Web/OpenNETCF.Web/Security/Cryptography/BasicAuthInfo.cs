using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Security.Cryptography
{
    public class BasicAuthInfo : IAuthenticationCallbackInfo
    {
        public string UserName { get; internal set; }
        public string Realm { get; internal set; }
        public string Uri { get; internal set; }
        public string Password { get; internal set; }
        public string Method { get; internal set; }
    }
}
