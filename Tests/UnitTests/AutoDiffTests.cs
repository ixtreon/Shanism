using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using Shanism.Engine;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Buffs;
using Shanism.Network.Client;
using Shanism.Network.Common;
using Shanism.Network.Serialization;
using Shanism.Network.Server;

namespace UnitTests
{
    [TestClass]
    public class AutoDiffTests
    {
        static GameObject[] testObjects = 
        {
            new Hero(Player.Aggressive) { BaseAgility = 13, Name = "Dummy" },
            new Unit { BaseMaxLife = 420, Position = new Vector(12, 13) },
            new Doodad { Model = "lala" },
            new Effect { CurrentTint = Color.Olive },
        };

        [TestMethod]
        public void TestAutoGenSerialization()
        {
            //var write2 = createWriter<Tst, Tst>();
            EntityMapper mapper = new EntityMapper();

            var msg = new NetBuffer();

            var w = new FieldWriter(msg);
            for (int i = 0; i < testObjects.Length; i++)
            {
                var testObj = testObjects[i];
                var defaultObj = mapper.GetDefault(testObj.ObjectType);
                mapper.Write(defaultObj, testObj, w);
            }

            msg.Position = 0;

            var outObjects = new ObjectStub[4];
            var r = new FieldReader(msg);
            for (uint i = 0; i < outObjects.Length; i++)
            {
                var baseObj = mapper.Create(testObjects[i].ObjectType, i + 1);
                outObjects[i] = baseObj;

                mapper.Read(baseObj, r);
            }

            Assert.AreEqual(
                ((Hero)testObjects[0]).Name,
                ((HeroStub)outObjects[0]).Name);

            Assert.AreEqual(
                ((Unit)testObjects[1]).BaseMaxLife,
                ((UnitStub)outObjects[1]).BaseStats[UnitStat.MaxLife]);
            Assert.AreEqual(
                ((Unit)testObjects[1]).Position,
                ((UnitStub)outObjects[1]).Position);

            Assert.AreEqual(
                ((Doodad)testObjects[2]).Model,
                ((DoodadStub)outObjects[2]).Model);

            Assert.AreEqual(
                ((Effect)testObjects[3]).CurrentTint,
                ((EffectStub)outObjects[3]).CurrentTint);


        }

        [TestMethod]
        public void TestBasicSerialization()
        {
            var msg = new NetBuffer();
            var writer = new ClientStateTracker();
            var reader = new ObjectCache();

            writer.WriteFrame(msg, 1, testObjects);
            reader.ReadFrame(msg);
        }


    }


}

