using Engine.Maps;
using IO;
using IO.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class HashMapPerfTest
    {
        static long TimeIt(Stopwatch sw, Action act)
        {
            sw.Reset();
            sw.Start();
            act();
            sw.Stop();
            return sw.ElapsedTicks;
        }


        static Random rnd = new Random(132);

        public TestContext TestContext { get; set; }


        [TestMethod]
        public void PerfTestConcurrentSeq()
        {
            const int ItemCount = 10000000;

            var items = Enumerable.Range(0, ItemCount)
                .Select(_ => rnd.NextVector())
                .ToArray();

            var a = new ConcurrentQueue<Vector>();
            var b = new ConcurrentStack<Vector>();

            var sw = new Stopwatch();
            var resA = TimeIt(sw, () =>
            {
                foreach (var it in items)
                    a.Enqueue(it);
            });

            var resB = TimeIt(sw, () =>
            {
                b.PushRange(items);
            });

            TestContext.WriteLine("QUEUE: {0} \tSTACK: {1}", resA, resB);
        }

        [TestMethod]
        public void TestListPerformance()
        {
            const int ptsPerIter = 1000;
            const int n_tests = 10;

            for(int i = 0; i < n_tests; i++)
                testLists(ptsPerIter * (i+1), repeats: 100);
        }

        public void testLists(int pointCount, int repeats = 10)
        {
            const int queryCount = 100;
            const int queryRectSize = 10;
            const int areaLength = 1000;

            long[] resultAdd = new long[3];
            long[] resultQuery = new long[3];


            var sw = new Stopwatch();

            var rnd = new Random();

            TestContext.WriteLine("[ADD:QUERY], n={0}, area={1}, L then CM then TM", pointCount, queryRectSize * queryRectSize);

            for (int i = 0; i < repeats; i++)
            {
                var list = new List<Vector>();
                var cMap = new Engine.Maps.Concurrent.HashMap<Vector>(new Vector(7));
                var tMap = new HashMap<Vector>(new Vector(7));

                var pts = Enumerable.Range(0, pointCount)
                    .Select(_ => new Vector(rnd.NextDouble(), rnd.NextDouble()) * areaLength)
                    .ToArray();

                var rects = Enumerable.Range(0, queryCount)
                    .Select(_ => new Rectangle((rnd.NextVector() * (areaLength - queryRectSize)).ToPoint(), new Point(queryRectSize)))
                    .ToArray();

                resultAdd[0] += TimeIt(sw, () =>
                {
                    foreach (var p in pts)
                        list.Add(p);
                });

                resultAdd[1] += TimeIt(sw, () =>
                {
                    foreach (var p in pts)
                        cMap.Add(p, p);
                });

                resultAdd[2] += TimeIt(sw, () =>
                {
                    foreach (var p in pts)
                        tMap.Add(p, p);
                });


                //Range queries

                resultQuery[0] += TimeIt(sw, () =>
                {
                    foreach (var rect in rects)
                    {
                        var count = 0;
                        foreach (var p in list)
                            if (p.Inside(rect.Position, rect.Size))
                                count++;
                    }
                });

                resultQuery[1] += TimeIt(sw, () =>
                {
                    foreach (var rect in rects)
                    {
                        var count = cMap.RangeQuery(rect.Position, rect.Size).Count();
                    }
                });

                resultQuery[2] += TimeIt(sw, () =>
                {
                    foreach(var rect in rects)
                    {
                        var count = tMap.RangeQuery(rect.Position, rect.Size).Count();
                    }
                });
                
            }


            for(int i = 0; i < 3; i++)
            {
                resultQuery[i] /= repeats;
                resultAdd[i] /= repeats;
            }

            TestContext.WriteLine("[{0}|{1}]\t [{2}|{3}]\t [{4}|{5}]", 
                resultAdd[0], resultQuery[0], 
                resultAdd[1], resultQuery[1],
                resultAdd[2], resultQuery[2]);
        }

    }
}
