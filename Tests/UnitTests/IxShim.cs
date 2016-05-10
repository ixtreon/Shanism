using Shanism.Engine.Objects.Entities;
using Shanism.Engine.Serialization;
using Shanism.Common.Game;
using Shanism.Common.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Network.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

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
