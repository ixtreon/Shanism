using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class StructHackTest
    {


        [TestMethod]
        public void MyTestMethod()
        {
            var writer = new StatsSerializer();
            var d = new StatsData();

            d[BoolStat.IsDead] = true;
            d[FloatStat.Agility] = 42;
            d[IntStat.MoveSpeedPerc] = 13;

            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                writer.Save(ms, d);
                bytes = ms.ToArray();
            }

            StatsData dOut;
            using (var ms = new MemoryStream(bytes))
            {
                dOut = writer.Load(ms);
            }

            var isDead = dOut[BoolStat.IsDead];
            var ag = dOut[FloatStat.Agility];
            var mana = dOut[FloatStat.MaxMana];
            var mss = dOut[IntStat.MoveSpeedPerc];
        }
    }
}
