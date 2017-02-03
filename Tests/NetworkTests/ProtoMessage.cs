using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;

namespace NetworkTests
{
    [TestClass]
    public class ProtoMessage
    {
        [TestMethod]
        public void HandshakeTest()
        {
            var msg = new HandshakeInitMessage("Shanist");

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, msg);
                bytes = ms.ToArray();
            }

            IOMessage outMsg;
            using (var ms = new MemoryStream(bytes))
            {
                outMsg = Serializer.Deserialize<IOMessage>(ms);
            }

            Assert.IsTrue(outMsg is HandshakeInitMessage);
            Assert.AreEqual("Shanist",
                ((HandshakeInitMessage)outMsg).PlayerName);
        }

        [TestMethod]
        public void MapRequestTest()
        {
            var msg = new MapRequestMessage(new Shanism.Common.ChunkId(1, 1));

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, msg);
                bytes = ms.ToArray();
            }

            IOMessage outMsg;
            using (var ms = new MemoryStream(bytes))
            {
                outMsg = Serializer.Deserialize<IOMessage>(ms);
            }

            Assert.IsTrue(outMsg is MapRequestMessage);
            Assert.AreEqual(1,
                ((MapRequestMessage)outMsg).Chunk.Id.X);
            Assert.AreEqual(1,
                ((MapRequestMessage)outMsg).Chunk.Id.Y);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var obj = new Bar { B = "life" };

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                bytes = ms.ToArray();
            }

            Bar outObj;
            using (var ms = new MemoryStream(bytes))
            {
                outObj = Serializer.Deserialize<Foo>(ms) as Bar;
            }

            //Assert.AreEqual(42, outObj.A);
            Assert.AreEqual("life", outObj.B);
        }

        [ProtoContract]
        [ProtoInclude(1, typeof(Bar))]
        abstract class Foo
        {
            //[ProtoMember(2)]
            public abstract int A { get; }
        }

        [ProtoContract]
        class Bar : Foo
        {
            public override int A => 42;

            [ProtoMember(1)]
            public string B { get; set; }
        }
    }
}
