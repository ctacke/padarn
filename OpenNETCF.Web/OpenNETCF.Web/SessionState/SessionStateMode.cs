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

namespace OpenNETCF.Web.SessionState
{
    /// <summary>
    /// Specifies the session-state mode.
    /// </summary>
    public enum SessionStateMode
    {
        /// <summary>
        /// Session state is disabled.
        /// </summary>
        Off,
        /// <summary>
        /// Session state is in process with the Padarn host process.
        /// </summary>
        InProc,
        /// <summary>
        /// Session state is in process with an Padarn worker process.
        /// </summary>
        StateServer,
        /// <summary>
        /// Session state is using an out-of-process SQL Server database to store state information.
        /// </summary>
        SQLServer,
        /// <summary>
        /// Session state is using a custom data store to store session-state information.
        /// </summary>
        Custom
    }
}
