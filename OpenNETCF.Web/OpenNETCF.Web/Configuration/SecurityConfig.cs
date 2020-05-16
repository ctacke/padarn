using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Security config for the server (enabled protocols)
    /// </summary>
    public sealed class SecurityConfig
    {
        internal SecurityConfig()
        {
            this.Tls10 = true;
            this.Tls11 = true;
            this.Tls12 = true;
            this.ResumeSession = true;
            CipherList = new List<short>();
        }

        /// <summary>
        /// Get or the set the session resume capabilities on the server
        /// </summary>
        public bool ResumeSession { get; internal set; }

        /// <summary>
        /// Get or set the activation of TLS 1.0
        /// </summary>
        public bool Tls10 { get; internal set; }

        /// <summary>
        /// Get or set the activation of TLS 1.1
        /// </summary>
        public bool Tls11 { get; internal set; }

        /// <summary>
        /// Get or set the activation of TLS 1.2
        /// </summary>
        public bool Tls12 { get; internal set; }

        /// <summary>
        /// A comma-separated list of numeric cipher suites to enable.  This is Eldos-specific.
        /// See https://www.eldos.com/documentation/sbb/documentation/ref_cl_clientsslsocket_mtd_getciphersuites.html
        /// </summary>
        public List<short> CipherList { get; internal set; }
    }
}
