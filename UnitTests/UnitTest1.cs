using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IO.Common;
using IO;
using System.Diagnostics;
using IO.Message;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            TerrainType[,] tutka = new TerrainType[5, 5];

            var protoArr = tutka.ToProtoArray<TerrainType>();

            var oldArr = protoArr.ToArray();
        }

        [TestMethod]
        public void testEnumConvert()
        {
            MessageType msg = MessageType.Action;

            var u = msg.GetShortValue();

            Assert.AreEqual((short)msg, u);
        }
    }
}
