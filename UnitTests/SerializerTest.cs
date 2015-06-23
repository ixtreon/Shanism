using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Objects.Serializers;

namespace UnitTests
{
    [TestClass]
    public class SerializerTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestPrimitives()
        {
            NetBuffer buf = new NetBuffer();

            int i = 51234;
            double d = 123.4567;
            string s = "abcdefghiwxyz";
            byte b = 35;
            char c = 'x';
            float f = 3.142857f;

            Serializer.TryWrite(buf, i);
            Serializer.TryWrite(buf, d);
            Serializer.TryWrite(buf, s);
            Serializer.TryWrite(buf, b);
            Serializer.TryWrite(buf, c);
            Serializer.TryWrite(buf, f);

            Assert.AreEqual(i, Serializer.Read<int>(buf));
            Assert.AreEqual(d, Serializer.Read<double>(buf));
            Assert.AreEqual(s, Serializer.Read<string>(buf));
            Assert.AreEqual(b, Serializer.Read<byte>(buf));
            Assert.AreEqual(c, Serializer.Read<char>(buf));
            Assert.AreEqual(f, Serializer.Read<float>(buf));

        }

        private enum testEnum
        {
            ok, notok
        }

        private enum testEnum2 : byte
        {
            notok, fuckinsux
        }

        [TestMethod]
        public void TestEnums()
        {
            NetBuffer buf = new NetBuffer();

            var a = testEnum.ok;
            Serializer.TryWrite(buf, a);
            Assert.AreEqual(a, Serializer.Read<testEnum>(buf));
        }

        [TestMethod]
        public void TestByteEnums()
        {
            NetBuffer buf = new NetBuffer();

            var b = testEnum2.fuckinsux;
            Serializer.TryWrite(buf, b);
            Assert.AreEqual(b, Serializer.Read<testEnum2>(buf));
        }
    }
}
