using NUnit.Framework;
using Shanism.Common.Util;

namespace UnitTests.Common
{
    [TestFixture]
    public class ShanoPathTests
    {
        [Test]
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
