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
        static readonly Effect effect;
        static readonly Doodad doodad;
        static readonly Unit unit;
        static readonly Hero hero;

        static GameObject[] testObjects;

        static AutoDiffTests()
        {
            effect = new Effect { CurrentTint = Color.Olive };
            doodad = new Doodad { Model = "lala", Position = new Vector(12, 13) };
            unit = new Unit { BaseMaxLife = 420 };
            hero = new Hero(Player.Aggressive) { BaseAgility = 13, Name = "Dummy", Experience = 0 };

            testObjects = new GameObject[] { hero, unit, doodad, effect, };
        }

        [TestMethod]
        public void MapperBasicEntitySerialization()
        {
            ObjectMapper mapper = new ObjectMapper();

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

            Assert.AreEqual(hero.Name,
                ((HeroStub)outObjects[0]).Name);

            Assert.AreEqual(unit.BaseMaxLife,
                ((UnitStub)outObjects[1]).BaseStats[UnitStat.MaxLife]);

            Assert.AreEqual(doodad.Position,
                ((DoodadStub)outObjects[2]).Position);
            Assert.AreEqual(doodad.Model,
                ((DoodadStub)outObjects[2]).Model);

            Assert.AreEqual(effect.CurrentTint,
                ((EffectStub)outObjects[3]).CurrentTint);
        }

        [TestMethod]
        public void InitialObjectSend()
        {
            var b = new Hero(Player.Aggressive) { BaseAgility = 42, BaseIntellect = 13, BaseStrength = 5 };
            var objs = new List<IGameObject> { b };

            var msg = new NetBuffer();
            var writer = new ClientStateTracker(0);
            var reader = new ObjectCache(0);

            writer.WriteFrame(msg, 1, objs);
            msg.Position = 0;
            reader.ReadFrame(msg);

            var bOut = reader.VisibleEntities.SingleOrDefault() as HeroStub;
            Assert.AreEqual(bOut.BaseAttributes[HeroAttribute.Agility], 42);
            Assert.AreEqual(bOut.BaseAttributes[HeroAttribute.Intellect], 13);
            Assert.AreEqual(bOut.BaseAttributes[HeroAttribute.Strength], 5);
        }


        [TestMethod]
        public void ContinuousPvsTracking()
        {
            var writer = new ClientStateTracker(0);
            var reader = new ObjectCache(0);
            var h = testObjects[0] as Hero;

            {
                var frameId = 1u;
                var msg = new NetBuffer();
                writer.WriteFrame(msg, frameId, new[] { h });

                reader.ReadFrame(msg);
                writer.SetLastAck(frameId);

                Assert.IsTrue(reader.VisibleEntities.Single() is HeroStub);
            }

            {
                var frameId = 2u;
                var msg = new NetBuffer();
                writer.WriteFrame(msg, frameId, new[] { h });

                reader.ReadFrame(msg);
                writer.SetLastAck(frameId);

                Assert.IsTrue(reader.VisibleEntities.Single() is HeroStub);
            }

            {
                var frameId = 3u;
                var msg = new NetBuffer();
                writer.WriteFrame(msg, frameId, new Hero[] { });

                reader.ReadFrame(msg);
                writer.SetLastAck(frameId);

                Assert.IsFalse(reader.VisibleEntities.Any());
            }

            {
                var frameId = 4u;
                var msg = new NetBuffer();
                writer.WriteFrame(msg, frameId, new[] { h });

                reader.ReadFrame(msg);
                writer.SetLastAck(frameId);

                Assert.IsTrue(reader.VisibleEntities.Single() is HeroStub);
            }
        }

        [TestMethod]
        public void ServerFrames_TestWriter()
        {
            var writer = new ClientStateTracker(0);
            var reader = new ObjectCache(0);

            var rnd = new Random(12345);
            var h = testObjects[0] as Hero;
            var clientExp = 0;

            //try to send a server frame N times
            for (uint i = 1; i <= 10; i++)
            {
                var msg = new NetBuffer();


                if (rnd.Next(2) == 0)
                    h.Experience = (int)i;

                writer.WriteFrame(msg, i, testObjects);

                //connection could be bad
                if (rnd.Next(2) == 0)
                {
                    reader.ReadFrame(msg);
                    writer.SetLastAck(i);

                    clientExp = h.Experience;
                }

                var outHero = reader.VisibleEntities.OfType<HeroStub>().Single();
                Assert.AreEqual(clientExp, outHero.Experience);
            }
        }


    }


}

