using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// Defines a User.
    /// </summary>
    public sealed class User
    {
        private string m_username;
        private string m_password;
        private RoleCollection m_roles;

        /// <summary>
        /// The user's username.
        /// </summary>
        public string Name
        {
            get { return m_username; }
            set { m_username = value; }
        }

        /// <summary>
        /// The user's password.
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        internal RoleCollection Roles
        {
            get { return m_roles; }
            set { m_roles = value; }
        }

        /// <summary>
        /// Creates an instance of the User class. 
        /// </summary>
        public User()
        {
            m_roles = new RoleCollection();
        }
    }
}
