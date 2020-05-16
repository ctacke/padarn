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
