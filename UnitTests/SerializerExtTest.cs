using IO.Common;
using IxSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class SerializerExtTest
    {
        public TestContext TestContext { get; set; }

        static SerializerExtTest()
        {
            Network.Objects.Serializers.SerializerModules.Init();
        }

        [TestMethod]
        public void TestVectors()
        {
            var v = new Vector(5, 10);

            var bytes = Serializer.GetWriter(w => Serializer.TryWrite(w, v));

            var vOut = Serializer.GetReader(bytes, r => Serializer.Read<Vector>(r));

            Assert.AreEqual(v.X, vOut.X);
            Assert.AreEqual(v.Y, vOut.Y);
        }
    }
}
