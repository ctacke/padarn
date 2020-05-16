using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Security.Principal
{
    /// <summary>
    /// Defines the basic functionality of an identity object.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        string AuthenticationType { get; }
        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        string Name { get; }
    }
}
