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
using System.Text;
using OpenNETCF.Configuration;
using OpenNETCF.Web.Configuration;
using OpenNETCF.Security;
using OpenNETCF.Web.Security.Cryptography;
using OpenNETCF.Security.Principal;

namespace OpenNETCF.Web.Security
{
    internal class BasicAuthentication : Authentication
    {
        static char[] separator = { ':' };
        
        private string m_user;

        public BasicAuthentication() : base("Basic")
        {
        }

        public override bool AcceptCredentials(HttpContext context, string authentication)
        {
            bool auth = true;

            byte[] userpass = Convert.FromBase64String(authentication);
            string[] up = Encoding.UTF8.GetString(userpass, 0, userpass.Length).Split(separator);
            m_user = up[0];
            string password = up[1];

            if (String.IsNullOrEmpty(this.User)) return false;

            var config = ServerConfig.GetConfig(); 
            if (config.Authentication.AuthenticationCallback == null)
            {
                auth = CheckConfigUserList(this.User, password);
            }
            else
            {
                var info = new BasicAuthInfo
                {
                    UserName = this.User,
                    Password = password,
                    Realm = config.Authentication.Realm,
                    Uri = context.Request.Path,
                    Method = context.Request.HttpMethod
                };

                auth = CheckUserWithServerCallback(info);
            }

            // set the user info
            var id = new GenericIdentity(User, this.AuthenticationMethod.ToLower());
            id.IsAuthenticated = auth;
            var principal = new GenericPrincipal(id);
            context.User = principal;

            return auth;
        }

        private bool CheckUserWithServerCallback(BasicAuthInfo info)
        {
            try
            {
                return ServerConfig.GetConfig().Authentication.AuthenticationCallback(info);
            }
            catch
            {
                return false;
            }
        }

        private bool CheckConfigUserList(string userName, string password)
        {
            User user;
            if ((user = ServerConfig.GetConfig().Authentication.Users.Find(userName)) == null)
            {
                return false;
            }

            if (!user.Password.Equals(password))
            {
                return false;
            }

            return true;
        }

        public override void OnEndRequest(object sender, EventArgs e)
        {
            HttpContext context = (HttpContext)sender;
            if (!AuthenticationRequired) return;

            string realm = ServerConfig.GetConfig().Authentication.Realm;
            string challenge = String.Format("{0} realm=\"{1}\"", AuthenticationMethod, realm);
            context.Response.AppendHeader("WWW-Authenticate", challenge);
        }

        internal override string User
        {
            get { return m_user; }
        }
    }
}
