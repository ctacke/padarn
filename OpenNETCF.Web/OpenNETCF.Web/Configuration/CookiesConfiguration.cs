namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Configures properties for cookies used by a Web application. 
    /// </summary>
    public sealed class CookiesConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether Secure Sockets Layer (SSL) communication is required. 
        /// </summary>
        public bool RequireSSL
        {
            get; internal set;
        }

        /// <summary>
        /// Gets or sets the cookie domain name. 
        /// </summary>
        public string Domain
        { 
            get; internal set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether the support for the browser's HttpOnly cookie is enabled. 
        /// </summary>
        public bool HttpOnlyCookies
        {
            get; internal set;
        }
    }
}
