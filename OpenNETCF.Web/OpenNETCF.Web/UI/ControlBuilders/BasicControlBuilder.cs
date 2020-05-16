using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI.WebControls;
using System.Collections;

namespace OpenNETCF.Web.UI
{
    internal class BasicControlBuilder<T> : ControlBuilder
        where T : WebControl
    {
        public BasicControlBuilder(string tagName)
        {
            this.ControlType = typeof(T);
            this.TagName = tagName;
        }

        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            if (String.Compare(tagName, this.TagName, true) == 0)
            {
                return typeof(T);
            }
            return null;
        }
    }
}
