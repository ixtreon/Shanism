using Engine.Objects;
using Engine.Objects.Entities;
using IO;
using IO.Common;
using IO.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ProtoShim
    {

        #region basic test



        //[TestMethod]
        //public void SurrogateTest()
        //{
        //    var dodo = new Monster { Name = "lalqlqlql", ManaPercentage = 0.55 };

        //    byte[] buffer;
        //    using (var ms = new MemoryStream())
        //    {
        //        var stubDodo = (UnitStub)dodo;
        //        Serializer.Serialize(ms, stubDodo);
        //        buffer = ms.ToArray();
        //    }


        //    UnitStub stubche;
        //    using (var ms = new MemoryStream(buffer))
        //    {
        //        stubche = Serializer.Deserialize<UnitStub>(ms);
        //    }

        //}

        #endregion

        [TestMethod]
        public void RectangleSerializeTest()
        {
            var rect = new RectangleF(1, 2, 3, 4);
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, rect);
                buffer = ms.ToArray();
            }


            RectangleF stubche;
            using (var ms = new MemoryStream(buffer))
            {
                stubche = Serializer.Deserialize<RectangleF>(ms);
            }

        }


        void addDoodadMapping()
        {

            var ps = typeof(IDoodad).GetAllProperties()
                .Where(p => p.Name != "ObjectType")
                .OrderBy(p => p.Name)
                .ToList();

            {
                var dTy = RuntimeTypeModel.Default.Add(typeof(Doodad), false);
                var i = 0;
                foreach (var p in ps)
                    dTy.AddField(++i, p.Name);
            }

            {
                var dTy = RuntimeTypeModel.Default.Add(typeof(DoodadStub), false);
                var i = 0;
                foreach (var p in ps)
                    dTy.AddField(++i, p.Name);
            }
        }


        [TestMethod]
        public void PerfTestProtoVsIx()
        {
            const int count = 1000000;

            addDoodadMapping();

            var go = new Doodad { Position = new Vector(10, 20), Name = "peso" };

            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                {
                    byte[] buffer;
                    using (var ms = new MemoryStream())
                    {
                        Serializer.Serialize(ms, go);
                        buffer = ms.ToArray();
                    }
                }
                sw.Stop();
                Console.WriteLine($"Proto: {sw.ElapsedTicks} ticks");
            }

            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                {
                    byte[] buffer2;
                    using (var ms = new MemoryStream())
                    {
                        using (var w = new BinaryWriter(ms))
                            Engine.Serialization.ShanoReader.SerializeObject(w, go);
                        buffer2 = ms.ToArray();
                    }
                }
                sw.Stop();
                Console.WriteLine($"Ix: {sw.ElapsedTicks} ticks");
            }
        }

        [TestMethod]
        public void TestProtoMethods()
        {
            addDoodadMapping();

            var go = new Doodad { Position = new Vector(10, 20), Name = "peso" };

            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, go);
                buffer = ms.ToArray();
            }

            byte[] buffer2;
            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                    Engine.Serialization.ShanoReader.SerializeObject(w, go);
                buffer2 = ms.ToArray();
            }


            Doodad obj;
            using (var ms = new MemoryStream(buffer))
            {
                obj = Serializer.Deserialize<Doodad>(ms);
            }

            DoodadStub obstub;
            using (var ms = new MemoryStream(buffer))
            {
                obstub = Serializer.Deserialize<DoodadStub>(ms);
            }
        }


        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        class FooIn
        {
            public int X { get; set; }

            public int Y { get; set; }
        }

        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        class FooOut
        {
            public int Y { get; set; }

            public int X { get; set; }
        }

        [TestMethod]
        public void CheckDiffObjs()
        {
            var fIn = new FooIn { X = 42 };

            byte[] datas;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, fIn);
                datas = ms.ToArray();
            }

            FooOut fOut;
            using (var ms = new MemoryStream(datas))
                fOut = Serializer.Deserialize<FooOut>(ms);

        }
    }
}
