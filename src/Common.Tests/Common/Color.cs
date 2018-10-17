using NUnit.Framework;

using static Shanism.Common.Color;

namespace UnitTests.Common
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void Packing()
        {
            Assert.IsTrue(Black == FromPacked(Black.Pack()));
            Assert.IsTrue(Pink == FromPacked(Pink.Pack()));
            Assert.IsTrue(Yellow == FromPacked(Yellow.Pack()));
        }
    }
}
