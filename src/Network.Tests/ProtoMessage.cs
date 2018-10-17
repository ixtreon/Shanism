using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Shanism.Common.Messages;
using NUnit.Framework;

namespace NetworkTests
{
    [TestFixture]
    public class ProtoMessage
    {
        [Test]
        public void HandshakeTest()
        {
            var msg = new HandshakeInit("Shanist");

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, msg);
                bytes = ms.ToArray();
            }

            ClientMessage outMsg;
            using (var ms = new MemoryStream(bytes))
            {
                outMsg = Serializer.Deserialize<ClientMessage>(ms);
            }

            Assert.IsTrue(outMsg is HandshakeInit);
            Assert.AreEqual("Shanist",
                ((HandshakeInit)outMsg).PlayerName);
        }

        [Test]
        public void MapRequestTest()
        {
            var msg = new MapRequest(new Shanism.Common.ChunkId(1, 1));

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, msg);
                bytes = ms.ToArray();
            }

            ClientMessage outMsg;
            using (var ms = new MemoryStream(bytes))
            {
                outMsg = Serializer.Deserialize<ClientMessage>(ms);
            }

            Assert.IsTrue(outMsg is MapRequest);
            Assert.AreEqual(1,
                ((MapRequest)outMsg).Chunk.ID.X);
            Assert.AreEqual(1,
                ((MapRequest)outMsg).Chunk.ID.Y);
        }

        [Test]
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
