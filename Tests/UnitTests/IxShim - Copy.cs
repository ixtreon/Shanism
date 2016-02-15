using Engine.Entities;
using Engine.Entities.Objects;
using IO;
using IO.Common;
using IO.Objects;
using IO.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Network.Objects;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{

    [TestClass]
    public class IxShim2
    {


        const int NReps = 1000000;

        [TestMethod]
        public void Tashaksansss()
        {
            var go = new Doodad { Position = new Vector(10, 20), Name = "peso" };
            byte[] buffer = null;

            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < NReps; i++)
                    using (var ms = new MemoryStream())
                    {
                        ProtoConverter.Default.Serialize(ms, go);
                        buffer = ms.ToArray();
                    }
                sw.Stop();

                Console.WriteLine("Serialize: {0} per second. ", NReps * 1000 / sw.ElapsedMilliseconds);
            }

            var obstub = new ObjectStub();
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < NReps; i++)
                    using (var ms = new MemoryStream(buffer))
                    {
                        ProtoConverter.Default.Deserialize(ms, obstub);
                    }
                sw.Stop();

                Console.WriteLine("Deserialize: {0} per second. ", NReps * 1000 / sw.ElapsedMilliseconds);
            }

        }
    }


}
