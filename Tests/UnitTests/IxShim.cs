using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Objects;
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


        const int NReps = 1000000;

        [TestMethod]
        public void Tashaksansss()
        {
            var go = new Monster { Position = new Vector(10, 20), Name = "peso" };
            byte[] buffer = null;

            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                {
                    ShanoReader.SerializeObject(w, go);
                }
                buffer = ms.ToArray();
            }

            var ngo = new UnitStub();

            using (var ms = new MemoryStream(buffer))
            using (var r = new BinaryReader(ms))
            {
                ShanoWriter.DeserializeObject(r, ngo);
            }

        }
    }


}
