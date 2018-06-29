using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.UI;
using OpenNETCF.Web.Html;
using OpenNETCF.Web.Html.Meta;

namespace SampleSite
{
    public class Time : Page
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0));
            Response.Cache.SetNoStore();
            Response.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            Response.Flush();
        }
    }
}
