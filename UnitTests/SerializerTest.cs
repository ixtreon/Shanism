using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Network.Objects.Serializers;
using IxSerializer;

namespace UnitTests
{
    [TestClass]
    public class SerializerTest
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Tests whether IxSerializer reads and writes primitives. 
        /// </summary>
        [TestMethod]
        public void TestPrimitives()
        {
            var ms = new MemoryStream();

            int i = 51234;
            double d = 123.4567;
            string s = "abcdefghiwxyz";
            byte b = 35;
            char c = 'x';
            float f = 3.142857f;

            using (var buf = new BinaryWriter(ms))
            {
                Serializer.TryWrite(buf, i);
                Serializer.TryWrite(buf, d);
                Serializer.TryWrite(buf, s);
                Serializer.TryWrite(buf, b);
                Serializer.TryWrite(buf, c);
                Serializer.TryWrite(buf, f);
            }

            ms = new MemoryStream(ms.ToArray());

            using (var buf = new BinaryReader(ms))
            {
                Assert.AreEqual(i, Serializer.Read<int>(buf));
                Assert.AreEqual(d, Serializer.Read<double>(buf));
                Assert.AreEqual(s, Serializer.Read<string>(buf));
                Assert.AreEqual(b, Serializer.Read<byte>(buf));
                Assert.AreEqual(c, Serializer.Read<char>(buf));
                Assert.AreEqual(f, Serializer.Read<float>(buf));
            }
        }

        private enum testEnum
        {
            ok, notok
        }

        private enum testEnum2 : byte
        {
            oksux, lolok
        }

        /// <summary>
        /// Tests whether IxSerializer works on enum values
        /// </summary>
        [TestMethod]
        public void TestEnums()
        {
            var a = testEnum.ok;
            var bytes = Serializer.GetWriter(w => 
                Serializer.TryWrite(w, a));

            Serializer.GetReader(bytes, r =>
            {
                Assert.AreEqual(a, Serializer.Read<testEnum>(r));
            });
        }

        /// <summary>
        /// Test whether IxSerializer works with custom-sized enums
        /// </summary>
        [TestMethod]
        public void TestByteEnums()
        {
            var a = testEnum2.oksux;

            var bytes = Serializer.GetWriter(w => 
                Serializer.TryWrite(w, a));

            Serializer.GetReader(bytes, r =>
            {
                Assert.AreEqual(a, Serializer.Read<testEnum2>(r));
            });
        }
    }
}
