using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Configuration
{
    /// <summary>
    /// A collection of Users.
    /// </summary>
    public sealed class UserCollection : List<User>
    {
        private static string searchTerm;

        /// <summary>
        /// Find a user with a specific username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User Find(string username)
        {
            searchTerm = username;
            return Find(FindByUserName);
        }

        static bool FindByUserName(User u)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return false;

            if (u.Name.Equals(searchTerm))
                return true;

            return false;
        }
    }
}
