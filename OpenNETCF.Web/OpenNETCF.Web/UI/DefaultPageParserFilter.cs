using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI
{
    class DefaultPageParserFilter : PageParserFilter
    {
        protected override string VirtualPath
        {
            get { throw new NotImplementedException(); }
        }

        public override bool AllowCode
        {
            get { throw new NotImplementedException(); }
        }
    }
}
