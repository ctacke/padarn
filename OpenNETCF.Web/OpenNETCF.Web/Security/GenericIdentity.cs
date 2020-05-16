using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Security.Principal
{
    /// <summary>
    /// Represents a generic user.
    /// </summary>
    public class GenericIdentity : IIdentity
    {
        /// <summary>
        /// Gets the type of authentication used to identify the user.
        /// </summary>
        public string AuthenticationType { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the user has been authenticated.
        /// </summary>
        public bool IsAuthenticated { get; internal set; }
        /// <summary>
        /// Gets the user's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GenericIdentity class representing the user with the specified name.
        /// </summary>
        /// <param name="userName"></param>
        public GenericIdentity(string userName)
            : this(userName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GenericIdentity class representing the user with the specified name and authentication type.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="authType"></param>
        public GenericIdentity(string userName, string authType)
        {
            Name = userName;
            AuthenticationType = authType;
            IsAuthenticated = false;
        }
    }
}
