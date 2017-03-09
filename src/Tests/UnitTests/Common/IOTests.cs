using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Common
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class IOTests
    {
        [TestMethod]
        public void TestSubfolders()
        {
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", ""));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "foo"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "foo"));

            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "/"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "/foo"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "/foo/"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "/foo/bar"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("/foo/bar", "/foo/bar/"));

            Assert.IsTrue(ShanoPath.IsSubFolderOf("foo/bar", "/"));
            Assert.IsTrue(ShanoPath.IsSubFolderOf("foo/bar", "/foo/bar/"));

            Assert.AreEqual("foo/bar",
                ShanoPath.GetRelativePath("/foo/bar", "/"));

            Assert.AreEqual("bar", 
                ShanoPath.GetRelativePath("/foo\\bar", "/foo"));
        }
    }
}
