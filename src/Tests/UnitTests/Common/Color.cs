using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Common
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void TestColorPackUnpack()
        {
            Assert.IsTrue(Color.Black == new Color(Color.Black.Pack()));
            Assert.IsTrue(Color.Pink == new Color(Color.Pink.Pack()));
            Assert.IsTrue(Color.Yellow == new Color(Color.Yellow.Pack()));
        }
    }
}
