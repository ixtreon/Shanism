using IO.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Tests
{
    [TestFixture]
    class ChunkTest
    {
        [Test]
        public void TestVectorToChunk() // test small const values
        {
            var pt = new Vector(0.1, 0.1);

            Assert.IsTrue(MapChunkId.ChunkOf(pt).ChunkId == new Point(0, 0));

            pt = new Vector(-0.1, -0.1);
            Assert.IsTrue(MapChunkId.ChunkOf(pt).ChunkId == new Point(-1, -1));

            pt = new Vector(-0.1, 0.1);
            Assert.IsTrue(MapChunkId.ChunkOf(pt).ChunkId == new Point(-1, 0));

            pt = new Vector(0.1, -0.1);
            Assert.IsTrue(MapChunkId.ChunkOf(pt).ChunkId == new Point(0, -1));
        }
    }
}
