using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class XorTest
    {
        const int SIZE = 8192;
        const int REPS = 10000;

        public TestContext TestContext { get; set; }


        [TestMethod]
        public void TestMe()
        {

            masivA = new byte[SIZE];
            masivB = new byte[SIZE];
            outMasiv = new byte[SIZE];

            var rnd = new Random();
            rnd.NextBytes(masivA);
            rnd.NextBytes(masivB);


            Stopwatch.StartNew().Stop();

            foreach (var _ in Enumerable.Range(0, 10))
            {
                {
                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < REPS; i++)
                        methodB();
                    sw.Stop();
                    TestContext.WriteLine(sw.ElapsedTicks.ToString());
                }

                {
                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < REPS; i++)
                        methodC();
                    sw.Stop();
                    TestContext.WriteLine(sw.ElapsedTicks.ToString());
                }
                TestContext.WriteLine("");
            }

        }

        byte[] masivA, masivB;
        byte[] outMasiv;

        void methodA()
        {
            for (var i = 0; i < SIZE; i++)
                outMasiv[i] = (byte)(masivA[i] ^ masivB[i]);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct UnionArray
        {
            [FieldOffset(0)]
            public byte[] Bytes;

            [FieldOffset(0)]
            public long[] Longs;

            [FieldOffset(0)]
            public int[] Ints;
        }

        void methodB()
        {
            unA = new UnionArray { Bytes = masivA };
            unB = new UnionArray { Bytes = masivB };
            unOut = new UnionArray { Bytes = outMasiv };

            var long_sz = SIZE * sizeof(byte) / sizeof(long);

            for (var i = 0; i < long_sz; i++)
                unOut.Longs[i] = unA.Longs[i] ^ unB.Longs[i];
        }

        UnionArray unA, unB, unOut;

        void methodC()
        {
            unA = new UnionArray { Bytes = masivA };
            unB = new UnionArray { Bytes = masivB };
            unOut = new UnionArray { Bytes = outMasiv };

            var long_sz = SIZE * sizeof(byte) / sizeof(long);

            Parallel.ForEach(Partitioner.Create(0, long_sz), doShit);
        }

        void doShit(Tuple<int, int> part)
        {
            for (var i = part.Item1; i < part.Item2; i++)
                unOut.Ints[i] = unA.Ints[i] ^ unB.Ints[i];
        }
    }
}
