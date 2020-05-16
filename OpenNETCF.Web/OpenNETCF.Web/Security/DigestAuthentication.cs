// 
// Copyright (c) 2008 OpenNETCF Consulting, LLC. All rights reserved.
// 
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using OpenNETCF.Web.Configuration;

namespace OpenNETCF.Web.Security
{
    using System;
    using OpenNETCF.Security;
    using OpenNETCF.Web.Security.Cryptography;
    using OpenNETCF.Security.Principal;

    /// <summary>
    /// 
    /// </summary>
    internal class DigestAuthentication : Authentication
    {
        private readonly int nonceLifetime = 60;
        private string m_user; 

        /// <summary>
        /// 
        /// </summary>
        public DigestAuthentication() : base("Digest")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authentication"></param>
        /// <returns></returns>
        public override bool AcceptCredentials(HttpContext context, string authentication)
        {
            bool auth = false;

            var config = ServerConfig.GetConfig();
            DigestAuthInfo digest = new DigestAuthInfo(context.Request.HttpMethod);

            string[] elements = authentication.Split(',');

            foreach(string element in elements)
            {
                int splitIndex = element.IndexOf('=');
                string K = element.Substring(0, splitIndex).Trim(new char[] {' ', '\"'});
                string V = element.Substring(splitIndex+1).Trim(new char[] {' ', '\"'});
                digest.AddElement(K, V);
            }

            m_user = digest["username"];

            if (config.Authentication.AuthenticationCallback == null)
            {
                auth = CheckConfigUserList(digest);
            }
            else
            {
                auth = CheckUserWithServerCallback(digest);
            }

            // set the user info
            var id = new GenericIdentity(User, this.AuthenticationMethod.ToLower());
            id.IsAuthenticated = auth;
            var principal = new GenericPrincipal(id);
            context.User = principal;

            return auth;
        }

        private bool CheckUserWithServerCallback(DigestAuthInfo digest)
        {
            try
            {
                return ServerConfig.GetConfig().Authentication.AuthenticationCallback(digest);
            }
            catch
            {
                return false;
            }
        }

        private bool CheckConfigUserList(DigestAuthInfo digest)
        {
            AuthenticationConfiguration authConfig = ServerConfig.GetConfig().Authentication;
            var user = authConfig.Users.Find(digest.UserName);

            if (user == null) return false;

            var password = user.Password;
            var hash = digest.GetHashCode(password);

            return digest["response"].Equals(hash);

            //if (authConfig.Users.Find(User) == null)
            //{
            //    // Username does not exist configuration
            //    return false;
            //}

            //string realm = authConfig.Realm;

            //// Calculate the digest hashes (taken from RFC2617)

            //// A1 = unq(username-value) ":" unq(realm-value) ":" passwd
            //string A1 = String.Format("{0}:{1}:{2}", User, realm, password);
            //// H(A1) = MD5(A1)
            //string HA1 = MD5Hash(A1);

            //// A2 = method ":" digest-uri
            //string A2 = String.Format("{0}:{1}", method, digest["uri"]);
            //// H(A2) = MD5(A2)
            //string HA2 = MD5Hash(A2);

            //// KD(secret, data) = H(concat(secret, ":", data))
            //// if qop == auth:
            //// request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
            ////                              ":" nc-value
            ////                              ":" unq(cnonce-value)
            ////                              ":" unq(qop-value)
            ////                              ":" H(A2)
            ////                            ) <">
            //// if qop is not present,
            //// request-digest  = 
            ////           <"> < KD ( H(A1), unq(nonce-value) ":" H(A2) ) > <">
            //string unhashedDigest;
            //if (digest["qop"].Equals("auth"))
            //{
            //    unhashedDigest = String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
            //        HA1,
            //        digest["nonce"],
            //        digest["nc"],
            //        digest["cnonce"],
            //        digest["qop"],
            //        HA2);
            //}
            //else
            //{
            //    unhashedDigest = String.Format("{0}:{1}:{2}",
            //        HA1, digest["nonce"], HA2);
            //}

            //string hashedDigest = MD5Hash(unhashedDigest);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnEndRequest(object sender, EventArgs e)
        {
            HttpContext context = (HttpContext)sender;
            if (!AuthenticationRequired)
                return;

            string realm = ServerConfig.GetConfig().Authentication.Realm;
            string nonce = GetCurrentNonce();

            StringBuilder challenge = new StringBuilder("Digest realm=\"");
            challenge.Append(realm);
            challenge.Append("\"");
            challenge.Append(", nonce=\"");
            challenge.Append(nonce);
            challenge.Append("\"");
            challenge.Append(", opaque=\"0000000000000000\"");
            challenge.Append(", stale=");
            challenge.Append("false");
            challenge.Append(", algorithm=MD5");
            challenge.Append(", qop=\"auth\"");

            context.Response.AppendHeader("WWW-Authenticate", challenge.ToString());
        }

        private string GetCurrentNonce()
        {
            DateTime nonceTime = DateTime.Now.AddSeconds(nonceLifetime);
            byte[] expireBytes = Encoding.ASCII.GetBytes(nonceTime.ToString("G"));
            string nonce = Convert.ToBase64String(expireBytes);
            nonce = nonce.TrimEnd('=');
            
            return nonce;
        }

        internal override string User
        {
            get { return m_user; }
        }
    }
}
