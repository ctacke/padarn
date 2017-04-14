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

namespace OpenNETCF.Security.Principal
{
    /// <summary>
    /// Represents a generic principal.
    /// </summary>
    public class GenericPrincipal : IPrincipal
    {
        private string[] m_roles;

        /// <summary>
        /// Initializes a new instance of the GenericPrincipal class from a user identity and an array of role names to which the user represented by that identity belongs.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roles"></param>
        public GenericPrincipal(IIdentity identity, string[] roles)
        {
            Identity = identity;
            m_roles = roles;
        }

        /// <summary>
        /// Gets the GenericIdentity of the user represented by the current GenericPrincipal.
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GenericPrincipal class from a user identity and an array of role names to which the user represented by that identity belongs.
        /// </summary>
        /// <param name="identity"></param>
        public GenericPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        /// <summary>
        /// Determines whether the current GenericPrincipal belongs to the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public virtual bool IsInRole(string role)
        {
            foreach (var r in m_roles)
            {
                if (r == role) return true;
            }

            return false;
        }
    }
}
