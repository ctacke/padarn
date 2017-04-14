using System;

using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenNETCF.Web.Rest
{
    [TestClass]
    public class RestUriPartTest
    {
        
        //[TestMethod]
        //[Description("Proves GetRestUri can return multipart path.")]
        //public void GetRestUriMultipart()
        //{
        //    string path1 = "/path1/blah";
        //    string path2 = "/path2/blob";
        //    RestUriPart target = new RestUriPart();
        //    RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);
        //    var p1 = target.Add(path1);
        //    var p2 = target.Add(path2);
        //    RestUriPart actual = target.GetRestUri(path1);
        //    Assert.AreEqual(p1, actual);
        //    RestUriPart path1Uri = target.GetRestUri("/path1");

        //    RestUriPart p3 = path1Uri.Children.GetRestUri("blah");
        //    Assert.IsNotNull(p3);

        //}

        //[TestMethod]
        //[Description("Proves GetRestUri can return multipart path.")]
        //public void GetRestUriMultipartThreeLevels()
        //{
        //    string path1 = "/path1/blah/jello";
        //    string path2 = "/path2/blob/kelloggs";
        //    RestUriPart target = new RestUriPart();
        //    RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);
        //    var p1 = target.Add(path1);
        //    var p2 = target.Add(path2); 
        //    RestUriPart actual = target.GetRestUri(path1);
        //    Assert.AreEqual(p1, actual);
        //    actual = target.GetRestUri("/path1");
        //    Assert.IsNotNull(actual);
        //    actual = target.GetRestUri("path1");
        //    Assert.IsNotNull(actual);
        //    actual = target.GetRestUri("/path1/blah");
        //    Assert.IsNotNull(actual);
        //    actual = target.GetRestUri("path1/blah/");
        //    Assert.IsNotNull(actual);
        //    StringAssert.EndsWith(actual.Uri, "blah");
        //}

        //[TestMethod]
        //[Description("Proves GetRestUri can return multipart path.")]
        //public void GetRestUriWithParameter()
        //{
        //    string path1Part1 = "/path1";
        //    string path1 = path1Part1 + "/{blah}/path2";
        //    string path2 = "/path2/blob";
        //    RestUriPart target = new RestUriPart();
        //    RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);
        //    var p1 = target.Add(path1);
        //    var p2 = target.Add(path2);

        //    RestUriPart actual = target.GetRestUri(path1Part1);
        //    actual = actual.Children.GetRestUri("{blah}");

        //    Assert.IsNotNull(actual);
        //    Assert.AreEqual(RestUriPartType.Parameter, actual.PartType);

        //}

        [TestMethod]
        [Description("Proves GetRoot returns the correct part.")]
        public void GetRootNoAddPositive()
        {
            string path1 = "/path1/";
            string path2 = "/path2/";
            RestUriPart target = new RestUriPart();
            RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);
            
            var p1 = target.Add(path1);
            var p2 = target.Add(path2);

            RestUriPart actual = target.GetRoot(path1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(p1, actual);
        }


        [TestMethod]
        [Description("Proves Add with two parts adds correct.")]
        public void GetRootNoAddPositive2()
        {
            string path1 = "/path1/bob";
            string path2 = "/path2/claire";
            RestUriPart target = new RestUriPart();
            RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);

            var p1 = target.Add(path1);
            var p2 = target.Add(path2);

            RestUriPart actual = target.GetPart(path1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(p1, actual, "{0} != {1}", p1.Uri, actual.Uri);
        }

        [TestMethod]
        [Description("Proves GetPart returns the correct child.")]
        public void GetPartChild()
        {
            string path1 = "/path1/bob";
            string path2 = "/path2/claire";
            RestUriPart target = new RestUriPart();
            RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);

            var p1 = target.Add(path1);
            var p2 = target.Add(path2);

            RestUriPart actual = target.GetPart(path1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(p1, actual);
        }

        [TestMethod]
        [Description("Proves GetPart returns the correct child.")]
        public void GetPartChildWithReplaceable()
        {
            string path1 = "/path1/{bob}/blah";
            string path1ToGet = "/path1/hello/blah";
            string path2 = "/path2/claire";
            RestUriPart target = new RestUriPart();
            RestUriPart_Accessor a = RestUriPart_Accessor.AttachShadow(target);

            var p1 = target.Add(path1);
            var p2 = target.Add(path2);

            RestUriPart actual = target.GetPart(path1ToGet);

            Assert.IsNotNull(actual);
            Assert.AreEqual(p1, actual);
        }

    }
}
