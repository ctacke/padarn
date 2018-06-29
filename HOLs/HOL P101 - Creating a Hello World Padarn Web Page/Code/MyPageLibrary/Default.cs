using System;
using OpenNETCF.Web.UI;

namespace MyPageLibrary
{
  public class Default : Page
  {
    protected override void Page_Load(object sender, EventArgs e)
    {
      Response.WriteLine("<html><body><h1>Hello World!</h1></body></html>");
      Response.Flush();
    }
  }
}
