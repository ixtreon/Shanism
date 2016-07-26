using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Common;
using Shanism.Engine;
using Shanism.Engine.Entities;
using Shanism.Engine.GameSystems.Maps.Re;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class KdTests
    {
        [TestMethod]
        public void Simpel()
        {
            var es = new Entity[]
                {
                    new Monster { Position = new Vector(1, 1) },
                    new Monster { Position = new Vector(2, 2) },
                    new Monster { Position = new Vector(3, 6) },
                    new Monster { Position = new Vector(4, 1) },
                    new Monster { Position = new Vector(6, 3) },
                    new Monster { Position = new Vector(6, 5) },
                    new Monster { Position = new Vector(7, 4) },
                    new Monster { Position = new Vector(7, 6) },
                }.ToList();

            KdTree tr = new KdTree(es);

            var life = 42;
            life++;
        }
    }
}
