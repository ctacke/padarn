using OpenNETCF.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Web.Server;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Web.Integration.Test
{
  [TestClass()]
  public class StartupTests : TestBase
  {
    [Ignore]
    [TestMethod()]
    public void ConfigFileTest()
    {
      // TODO: copy over a config file

      WebServer server = new WebServer();
      server.Start();

      // TODO: check that no virtual directories does not throw
    }
  }
}
