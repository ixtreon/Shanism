using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class protobuftest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void TestProtoBufSize()
        {
            RuntimeTypeModel.Default.Add(typeof(IA), true)
                .Add()
                .AddSubType(1, typeof(IB));


            var cont = new Cont
            {
                Obj = new B { x = 1, y = 2, z = 123123 }
            };

            var cont2 = new B { x = 1, y = 2, z = 123123 };

            //serialize
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                //var cont22 = Serializer.ChangeType<B, A> (cont2);
                Serializer.Serialize<IA>(ms, cont2);
                buffer = ms.ToArray();
            }

            //deserialize
            IA outContainer;
            using (var ms = new MemoryStream(buffer))
                outContainer = Serializer.Deserialize<IA>(ms);
            //var outz = outContainer.Obj;

            TestContext.WriteLine(buffer.Length.ToString());
        }
    }

    [ProtoContract]
    class Cont
    {
        [ProtoMember(100)]
        public IA Obj;
    }

    [ProtoContract]
    interface IA
    {
        [ProtoMember(100)]
        int x { get; set; }
        [ProtoMember(101)]
        int y { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(1, typeof(B))]
    interface IB : IA
    {
        [ProtoMember(100)]
        int z { get; set; }
    }

    //[ProtoContract]
    //[ProtoInclude(1, typeof(B))]
    class A : IA
    {
        //[ProtoMember(100)]
        public int x { get; set; } = 42;
        //[ProtoMember(101)]
        public int y { get; set; } = 69;
    }

    //[ProtoContract]
    class B : A, IB
    {
        //[ProtoMember(200)]
        public int z { get; set; } = 56;
    }

    //[ProtoContract]
    //class C : A
    //{
    //    [ProtoMember(200)]
    //    public int w { get; set; } = 44;
    //}
    //[ProtoContract]
    //class D : A
    //{
    //    [ProtoMember(200)]
    //    public int asd { get; set; } = 123;
    //}
}
