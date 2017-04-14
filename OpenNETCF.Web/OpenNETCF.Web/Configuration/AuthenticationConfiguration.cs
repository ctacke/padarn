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
namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Descriptions the web server's authentication configuration
    /// </summary>
    public sealed class AuthenticationConfiguration
    {
        private string m_mode;
        private bool m_enabled;
        private string m_realm;
        private UserCollection m_users;

        /// <summary>
        /// Gets the authentication mode. Either Basic, Digest or Forms.
        /// </summary>
        public string Mode
        {
            get { return m_mode; }
            internal set { m_mode = value; }
        }

        /// <summary>
        /// Gets the authentication realm for HTTP Authentication.
        /// </summary>
        public string Realm
        {
            get { return m_realm; }
            internal set { m_realm = value; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether authentication is enabled or not. 
        /// </summary>
        public bool Enabled
        {
            get { return m_enabled; }
            internal set { m_enabled = value; }
        }

        /// <summary>
        /// Describes the users configuration for authentication.
        /// </summary>
        public UserCollection Users
        {
            get { return m_users; }
        }

        public AuthenticationCallbackHandler AuthenticationCallback { get; set; }

        /// <summary>
        /// Creates an instance of the AuthneticationConfiguration class. 
        /// </summary>
        public AuthenticationConfiguration()
        {
            m_users = new UserCollection();
        }
    }
}
