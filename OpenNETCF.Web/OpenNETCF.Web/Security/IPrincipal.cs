using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Security.Principal
{
    /// <summary>
    /// Defines the basic functionality of a principal object.
    /// </summary>
    public interface IPrincipal
    {
        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        IIdentity Identity { get; }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string role);

    }
}
