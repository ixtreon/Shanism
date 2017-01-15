using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using Shanism.Engine.Objects.Buffs;
using Shanism.Network;
using Shanism.Network.Client;
using Shanism.Network.Server;

namespace NetworkTests
{
    [TestClass]
    public class NetworkClientTests
    {
        [TestMethod]
        public void InitialObjectSend()
        {
            var b = new Buff { Agility = 42, Intellect = 13, Strength = 5 };
            var objs = new List<IGameObject> { b };

            var msg = new NetBuffer();
            var writer = new ClientStateTracker();
            var reader = new ObjectCache();

            writer.WriteFrame(msg, 1, objs);
            msg.Position = 0;
            reader.ReadFrame(msg);

            var bOut = reader.VisibleEntities.SingleOrDefault() as IBuff;
            Assert.AreEqual(bOut.Attributes[HeroAttribute.Agility], 42);
            Assert.AreEqual(bOut.Attributes[HeroAttribute.Intellect], 13);
            Assert.AreEqual(bOut.Attributes[HeroAttribute.Strength], 5);
        }



    }
}
