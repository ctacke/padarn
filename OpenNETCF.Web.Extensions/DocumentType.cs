using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.UI
{
    public enum DocumentType
    {
        HTML401Transitional,
        XHTML10Transitional,
    }

    public static class DoctypeTags
    {
        public const string HTML4 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">";
        public const string XHTML1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
    }
}
