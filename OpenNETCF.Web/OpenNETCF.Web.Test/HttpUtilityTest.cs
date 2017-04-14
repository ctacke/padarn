using OpenNETCF.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Web.Test
{
  [TestClass()]
  public class HttpUtilityTest// : TestBase
  {
    [Description("Tests that encoding and decoding of several strings with extended characters ends up with the source input")]
    [TestMethod()]
    public void UrlEncodeTest()
    {
      string[] testStrings = new string[]
        {
          "Hello World",  // english
          "Crédito",      // spanish
          "Préférences",  // french
          "高级搜索",       // chinese
          "언어도구",       // korean
          "検索オプション ",  // japanese
          "Настройки",    // russian
          "Προτιμήσεις",  // greek
          "æçèéêëìíîï",   // extended ascii 230-239
          "ðñòóôõö÷øù",   // extended ascii 240-249
          "úûüýþÿ"        // extended ascii 250-255
        };

      foreach (string s in testStrings)
      {
        string encoded = HttpUtility.UrlEncode(s);
        string decoded = HttpUtility.UrlDecode(encoded);
        Assert.AreEqual(s, decoded, string.Format("Failed to encode and decode '{0}'", s));
      }
    }
  }
}
