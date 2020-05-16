using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Defines the interface a control implements to get or set its text content.
    /// </summary>
    public interface ITextControl
    {
        /// <summary>
        /// Gets or sets the text content of a control.
        /// </summary>
        string Text { get; set; }
    }
}
