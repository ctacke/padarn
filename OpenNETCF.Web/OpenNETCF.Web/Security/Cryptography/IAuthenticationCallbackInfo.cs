using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Security.Cryptography
{
    public interface IAuthenticationCallbackInfo
    {
        string UserName { get; }
        string Realm { get; }
        string Uri { get; }
        string Method { get; }
    }
}
