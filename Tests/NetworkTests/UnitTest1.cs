using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using Shanism.Engine.Objects.Buffs;
using Shanism.Network;
using Shanism.Network.Common;
using Shanism.Network.Server;

namespace NetworkTests
{
    [TestClass]
    public class NetworkClientTests
    {

        [TestMethod]
        public void MyTestMethod()
        {
            const int max = 1000;
            for (int i = 0; i < max; i++)
            {
                var ang = 2 * (float)Math.PI * i / max;


                var ms1 = new MovementState(ang);
                var ms1b = ms1.GetDirectionByte();
                var ms2 = new MovementState(ms1b);
                var ms2b = ms2.GetDirectionByte();

                if (ms1b != ms2b)
                {
                    var life = 42;
                }

                Assert.AreEqual(ms1b, ms2b);
            }
        }


    }
}
