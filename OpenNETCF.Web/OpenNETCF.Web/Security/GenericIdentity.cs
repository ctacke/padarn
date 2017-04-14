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
