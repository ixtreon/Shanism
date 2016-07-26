using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Serialization;
using Shanism.Common.StubObjects;
using Shanism.Engine.Entities;
using Shanism.Engine.Serialization;
using Shanism.Network.Client;
using System.IO;
using System.Linq;

namespace UnitTests
{

    [TestClass]
    public class IxShim
    {
        GameSerializer sercho = new GameSerializer();

        Vector pos = new Vector(10, 20);

        [TestMethod]
        public void Tashaksansss()
        {
            var go = new Monster { Position = pos, Name = "peso" };
            byte[] buffer = null;

            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                {
                    sercho.Write(w, go, go.ObjectType);
                }
                buffer = ms.ToArray();
            }

            var ngo = new UnitStub();

            using (var ms = new MemoryStream(buffer))
            using (var r = new BinaryReader(ms))
            {
                var h = sercho.ReadHeader(r);
                sercho.Update(r, h, ngo);
            }

            Assert.AreEqual("peso", ngo.Name);
            Assert.AreEqual(pos, ngo.Name);
        }
    }


}
