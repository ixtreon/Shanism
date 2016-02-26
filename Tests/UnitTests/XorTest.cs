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
        const int NumBatches = 8;
        const int StartBatchSize = 1024;
        const int MaxBatchSize = StartBatchSize * (1 << NumBatches);
        const int REPS = 1000;



        public TestContext TestContext { get; set; }


        [TestMethod]
        public void TestMe()
        {

            masivA = new byte[MaxBatchSize];
            masivB = new byte[MaxBatchSize];
            outMasiv = new byte[MaxBatchSize];

            var rnd = new Random();
            rnd.NextBytes(masivA);
            rnd.NextBytes(masivB);


            Stopwatch.StartNew().Stop();

            foreach (var i in Enumerable.Range(0, NumBatches))
            {
                var sz = StartBatchSize * (1 << i);
                Console.WriteLine($"Batch Size: {sz}");

                {
                    var sw = Stopwatch.StartNew();
                    for (var _ = 0; _ < REPS; _++)
                        methodA(sz);
                    sw.Stop();
                    Console.Write($"A: {sw.ElapsedTicks}\t");
                }

                {
                    var sw = Stopwatch.StartNew();
                    for (var _ = 0; _ < REPS; _++)
                        methodB(sz);
                    sw.Stop();
                    Console.Write($"B: {sw.ElapsedTicks}\t");
                }

                {
                    var sw = Stopwatch.StartNew();
                    for (var _ = 0; _ < REPS; _++)
                        methodC(sz);
                    sw.Stop();
                    Console.Write($"C: {sw.ElapsedTicks}\t");
                }
                Console.WriteLine();
            }

        }

        /* Common */
        byte[] masivA, masivB;
        byte[] outMasiv;

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


        /* Method A - sequential xor */

        void methodA(int sz)
        {
            for (var i = 0; i < sz; i++)
                outMasiv[i] = (byte)(masivA[i] ^ masivB[i]);
        }


        /* Method B - union array */

        void methodB(int sz)
        {
            unA = new UnionArray { Bytes = masivA };
            unB = new UnionArray { Bytes = masivB };
            unOut = new UnionArray { Bytes = outMasiv };

            var long_sz = sz * sizeof(byte) / sizeof(long);

            for (var i = 0; i < long_sz; i++)
                unOut.Longs[i] = unA.Longs[i] ^ unB.Longs[i];
        }


        /* Method C - parallelized union array */

        UnionArray unA, unB, unOut;

        void methodC(int sz)
        {
            unA = new UnionArray { Bytes = masivA };
            unB = new UnionArray { Bytes = masivB };
            unOut = new UnionArray { Bytes = outMasiv };

            var long_sz = sz * sizeof(byte) / sizeof(long);

            Parallel.ForEach(Partitioner.Create(0, long_sz), doPartXor);
        }

        void doPartXor(Tuple<int, int> part)
        {
            for (var i = part.Item1; i < part.Item2; i++)
                unOut.Ints[i] = unA.Ints[i] ^ unB.Ints[i];
        }
    }
}
