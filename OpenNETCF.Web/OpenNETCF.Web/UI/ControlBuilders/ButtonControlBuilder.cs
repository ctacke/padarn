using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI.WebControls;
using System.Collections;

namespace OpenNETCF.Web.UI
{
    internal class ButtonControlBuilder : ControlBuilder
    {
        public ButtonControlBuilder()
        {
            this.ControlType = typeof(Button);
            this.TagName = "button";
        }

        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            if (String.Compare(tagName, this.TagName, true) == 0)
            {
                return typeof(Button);
            }
            return null;
        }
    }
}
