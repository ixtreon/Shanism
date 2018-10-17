using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shanism.Network.Serialization;
using NUnit.Framework;

namespace NetworkTests
{

    class Dummy
    {
        public string SA { get; } = "123123123";
        public string SB { get; } = "";
        public string SC { get; } = "JASDHkl12325HQILWEjaslkdjIYQUOWEY)72198372jhLKJSHKJDA";

        public int IA { get; } = 15;
        public int IB { get; } = 1123123123;
        public int IC { get; } = -23134;

        public float FA { get; } = 0.123123f;
        public float FB { get; } = -15125.123f;
        public float FC { get; } = 1273781263213123123.123f;
    }

    [TestFixture]
    class AutoPropertyGen
    {

        [Test]
        public void MyTestMethod()
        {

        }


        public void WriteFields(FieldWriter wr, Dummy d)
        {
            wr.WriteString(string.Empty, d.SA);
            wr.WriteString(string.Empty, d.SB);
            wr.WriteString(string.Empty, d.SC);

            wr.WriteInt(0, d.IA);
            wr.WriteInt(0, d.IB);
            wr.WriteInt(0, d.IC);

            wr.WriteFloat(0, d.FA);
            wr.WriteFloat(0, d.FB);
            wr.WriteFloat(0, d.FC);
        }
    }
}
