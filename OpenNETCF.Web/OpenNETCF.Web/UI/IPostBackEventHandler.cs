using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Defines the method Padarn server controls must implement to handle postback events.
    /// </summary>
    public interface IPostBackEventHandler
    {
        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A String that represents an optional event argument to be passed to the event handler.</param>
        void RaisePostBackEvent(string eventArgument);
    }
}
