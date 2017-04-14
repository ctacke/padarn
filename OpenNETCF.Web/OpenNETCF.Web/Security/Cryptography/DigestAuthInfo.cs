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
using System.Security.Cryptography;

namespace OpenNETCF.Web.Security.Cryptography
{
    public class DigestAuthInfo : IAuthenticationCallbackInfo
    {
        public string Method { get; private set; }
        private Dictionary<string, string> m_data;

        internal DigestAuthInfo(string httpMethod)
        {
            m_data = new Dictionary<string,string>();

            Method = httpMethod;
        }

        public string UserName 
        {
            get
            {
                return m_data["username"];
            }
        }

        public string Realm
        {
            get
            {
                return m_data["realm"];
            }
        }

        public string Uri
        {
            get
            {
                return m_data["uri"];
            }
        }

        internal void AddElement(string key, string value)
        {
            m_data.Add(key, value);
        }

        internal string this[string name]
        {
            get { return m_data[name]; }
        }

        /// <summary>
        /// Gets a unique MD5 hash code for the provides password based on RFC 2617
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetHashCode(string password)
        {
            // Calculate the digest hashes (taken from RFC2617)

            // A1 = unq(username-value) ":" unq(realm-value) ":" passwd
            var A1 = String.Format("{0}:{1}:{2}", UserName, Realm, password);
            // H(A1) = MD5(A1)
            var HA1 = MD5Hash(A1);

            // A2 = method ":" digest-uri
            var A2 = String.Format("{0}:{1}", Method, m_data["uri"]);
            // H(A2) = MD5(A2)
            var HA2 = MD5Hash(A2);

            // KD(secret, data) = H(concat(secret, ":", data))
            // if qop == auth:
            // request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
            //                              ":" nc-value
            //                              ":" unq(cnonce-value)
            //                              ":" unq(qop-value)
            //                              ":" H(A2)
            //                            ) <">
            // if qop is not present,
            // request-digest  = 
            //           <"> < KD ( H(A1), unq(nonce-value) ":" H(A2) ) > <">
            string unhashedDigest;
            if (m_data["qop"].Equals("auth"))
            {
                unhashedDigest = String.Format("{0}:{1}:{2}:{3}:{4}:{5}",
                    HA1,
                    m_data["nonce"],
                    m_data["nc"],
                    m_data["cnonce"],
                    m_data["qop"],
                    HA2);
            }
            else
            {
                unhashedDigest = String.Format("{0}:{1}:{2}",
                    HA1, m_data["nonce"], HA2);
            }

            var hashedDigest = MD5Hash(unhashedDigest);

            return hashedDigest;
        }

        /// <summary>
        /// Checks to see if the HashCode generated with the supplied password matches the hash stored in the Digest
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool MatchCredentials(string password)
        {
            return GetHashCode(password).Equals(m_data["response"]);
        }

        private static string MD5Hash(string str)
        {
            MD5 hash = MD5.Create();
            byte[] h = hash.ComputeHash(Encoding.ASCII.GetBytes(str));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in h)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
